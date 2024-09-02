using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DefaultConfig;
using static uGovernIT.DefaultConfig.Data.DefaultData.ConfigData;

namespace uGovernIT.DefaultConfig.Data.DefaultData
{
    public class Asset : IAssetModule
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
                    ShortName = "Asset Management",
                    CategoryName = "Resource Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "Assets",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/CMDB",
                    ModuleHoldMaxStage = 6,
                    Title = "Asset Management (CMDB)",
                    ModuleDescription = "This module is used to manage company assets such as desktops, laptops, tablets, PDAs, data center equipment, etc.",
                    ThemeColor = "Accent5",
                    StaticModulePagePath = "/Pages/Assetdetail",
                    ModuleType = ModuleType.Governance,
                    EnableModule = true,
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
                return "CMDB";
            }
        }

        public List<ACRType> GetACRTypes()
        {
            List<ACRType> mList = new List<ACRType>();

            return mList;
        }

        public List<AssetModel> GetAssetModels()
        {
            List<AssetModel> mList = new List<AssetModel>();
            // Console.WriteLine("  AssetModels");

            // Title, VendorLookup, ModelName, Description, BudgetLookup, NOT USED: BudgetCategory, BudgetSubCategory
            mList.Add(new AssetModel() { Title = "Apple - iMac", VendorLookup = 1, ModelName = "iMac", ModelDescription = "All in One PC" });
            mList.Add(new AssetModel() { Title = "Apple - iPad2 16 GB", VendorLookup = 1, ModelName = "iPad2 16 GB", ModelDescription = "Entry Level Touchpad" });
            mList.Add(new AssetModel() { Title = "Apple - iPad2 32 GB", VendorLookup = 1, ModelName = "iPad2 32 GB", ModelDescription = "Professional Tablet" });
            mList.Add(new AssetModel() { Title = "Apple - iPhone 4 16 GB", VendorLookup = 1, ModelName = "iPhone 4 16 GB", ModelDescription = "Entry level iPhone" });
            mList.Add(new AssetModel() { Title = "Apple - iPhone 4 32 GB", VendorLookup = 1, ModelName = "iPhone 4 32 GB", ModelDescription = "High end PDA" });
            mList.Add(new AssetModel() { Title = "Apple - MacBook Air", VendorLookup = 1, ModelName = "MacBook Air", ModelDescription = "Entry Light Laptop" });
            mList.Add(new AssetModel() { Title = "Apple - MacBook Pro", VendorLookup = 1, ModelName = "MacBook Pro", ModelDescription = "Professional Laptop" });
            mList.Add(new AssetModel() { Title = "Apple - MacPro", VendorLookup = 1, ModelName = "MacPro", ModelDescription = "High Performance PC" });
            mList.Add(new AssetModel() { Title = "Barracuda - NG Firewall", VendorLookup = 2, ModelName = "NG Firewall", ModelDescription = "Corporate " });
            mList.Add(new AssetModel() { Title = "Cisco - Model 2960", VendorLookup = 3, ModelName = "Model 2960", ModelDescription = "Site Office Switch" });
            mList.Add(new AssetModel() { Title = "Cisco - Model 3560", VendorLookup = 3, ModelName = "Model 3560", ModelDescription = "Regional Office Switch" });
            mList.Add(new AssetModel() { Title = "Cisco - Model 4500", VendorLookup = 3, ModelName = "Model 4500", ModelDescription = "Corporate Office Switch" });
            mList.Add(new AssetModel() { Title = "Dell - Inspiron", VendorLookup = 4, ModelName = "Inspiron", ModelDescription = "Efficient PC" });
            mList.Add(new AssetModel() { Title = "Dell - Lattitude", VendorLookup = 4, ModelName = "Lattitude", ModelDescription = "Basic Laptops" });
            mList.Add(new AssetModel() { Title = "Dell - Optiplex", VendorLookup = 4, ModelName = "Optiplex", ModelDescription = "Entry Level PC" });
            mList.Add(new AssetModel() { Title = "Dell - Power Edge 210", VendorLookup = 4, ModelName = "Power Edge 210", ModelDescription = "Entry server" });
            mList.Add(new AssetModel() { Title = "Dell - Power Edge C6105", VendorLookup = 4, ModelName = "Power Edge C6105", ModelDescription = "Mid-size server" });
            mList.Add(new AssetModel() { Title = "Dell - Streak 7 Android", VendorLookup = 4, ModelName = "Streak 7 Android", ModelDescription = "Professional Tablet" });
            mList.Add(new AssetModel() { Title = "Dell - Vostro", VendorLookup = 4, ModelName = "Vostro", ModelDescription = "Small Business" });
            mList.Add(new AssetModel() { Title = "Dell - XPS", VendorLookup = 4, ModelName = "XPS", ModelDescription = "High Performance PC" });
            mList.Add(new AssetModel() { Title = "Dell - Power Edge 415", VendorLookup = 4, ModelName = "Power Edge 415", ModelDescription = "High Performance server" });
            mList.Add(new AssetModel() { Title = "Dell - Streak 5 Android", VendorLookup = 4, ModelName = "Streak 5 Android", ModelDescription = "Entry Level Touchpad" });
            mList.Add(new AssetModel() { Title = "Fortinet - Model 200", VendorLookup = 4, ModelName = "Model 200", ModelDescription = "Corporate Firewall" });
            mList.Add(new AssetModel() { Title = "Fortinet - Model 80", VendorLookup = 4, ModelName = "Model 80", ModelDescription = "Site Office Firewall" });
            mList.Add(new AssetModel() { Title = "Google - Nexus S4G", VendorLookup = 4, ModelName = "Nexus S4G", ModelDescription = "PDA for Engineers" });
            mList.Add(new AssetModel() { Title = "HP - Envy 1 Series", VendorLookup = 4, ModelName = "Envy 1 Series", ModelDescription = "UltraLight Laptops" });
            mList.Add(new AssetModel() { Title = "HP - HPE h8i", VendorLookup = 4, ModelName = "HPE h8i", ModelDescription = "Efficient PC" });
            mList.Add(new AssetModel() { Title = "HP - HPE h8m", VendorLookup = 4, ModelName = "HPE h8m", ModelDescription = "Small Business" });
            mList.Add(new AssetModel() { Title = "HP - HPE h8s", VendorLookup = 4, ModelName = "HPE h8s", ModelDescription = "Entry Level PC" });
            mList.Add(new AssetModel() { Title = "HP - HPE h8t", VendorLookup = 4, ModelName = "HPE h8t", ModelDescription = "High Performance PC" });
            mList.Add(new AssetModel() { Title = "HP - Mini 110", VendorLookup = 4, ModelName = "Mini 110", ModelDescription = "Small Laptop" });
            mList.Add(new AssetModel() { Title = "HP - Pavilion dm1z", VendorLookup = 4, ModelName = "Pavilion dm1z", ModelDescription = "Portable laptops" });
            mList.Add(new AssetModel() { Title = "HP - Pavilion dv6z", VendorLookup = 4, ModelName = "Pavilion dv6z", ModelDescription = "High Performance Laptops" });
            mList.Add(new AssetModel() { Title = "HP - Probook 4530", VendorLookup = 4, ModelName = "Probook 4530", ModelDescription = "Basic Laptops" });
            mList.Add(new AssetModel() { Title = "HP - TouchPad 16 GB", VendorLookup = 4, ModelName = "TouchPad 16 GB", ModelDescription = "Entry Level Touchpad" });
            mList.Add(new AssetModel() { Title = "HP - TouchPad 32 GB", VendorLookup = 4, ModelName = "TouchPad 32 GB", ModelDescription = "Professional Tablet" });
            mList.Add(new AssetModel() { Title = "IBM - AMD 6176", VendorLookup = 4, ModelName = "AMD 6176", ModelDescription = "Blade Server" });
            mList.Add(new AssetModel() { Title = "IBM - H Chasis", VendorLookup = 4, ModelName = "H Chasis", ModelDescription = "Chasis With Power" });
            mList.Add(new AssetModel() { Title = "IBM - Intel Core i7 2600", VendorLookup = 4, ModelName = "Intel Core i7 2600", ModelDescription = "Blade Server" });
            mList.Add(new AssetModel() { Title = "IBM - Intel Core i7 980X", VendorLookup = 4, ModelName = "Intel Core i7 980X", ModelDescription = "Blade Server" });
            mList.Add(new AssetModel() { Title = "IBM - Intel Core i7 990X", VendorLookup = 4, ModelName = "Intel Core i7 990X", ModelDescription = "Blade Server" });
            mList.Add(new AssetModel() { Title = "IBM - Intel Core i7 995X", VendorLookup = 4, ModelName = "Intel Core i7 995X", ModelDescription = "Blade Server" });
            mList.Add(new AssetModel() { Title = "IBM - Intel Core W3680", VendorLookup = 4, ModelName = "Intel Core W3680", ModelDescription = "Blade Server" });
            mList.Add(new AssetModel() { Title = "IBM - Intel Core X5630", VendorLookup = 4, ModelName = "Intel Core X5630", ModelDescription = "Blade Server" });
            mList.Add(new AssetModel() { Title = "IBM - Intel Core X5650", VendorLookup = 4, ModelName = "Intel Core X5650", ModelDescription = "Blade Server" });
            mList.Add(new AssetModel() { Title = "IBM - Intel Core X5690", VendorLookup = 4, ModelName = "Intel Core X5690", ModelDescription = "Blade Server" });
            mList.Add(new AssetModel() { Title = "IBM - Intel Core X7560", VendorLookup = 4, ModelName = "Intel Core X7560", ModelDescription = "Blade Server" });
            mList.Add(new AssetModel() { Title = "RIM - Bold 9900", VendorLookup = 4, ModelName = "Bold 9900", ModelDescription = "High end PDA" });
            mList.Add(new AssetModel() { Title = "RIM - Curve 3G", VendorLookup = 4, ModelName = "Curve 3G", ModelDescription = "PDA for Managers" });
            mList.Add(new AssetModel() { Title = "RIM - Curve 8900", VendorLookup = 4, ModelName = "Curve 8900", ModelDescription = "Executive PDA" });
            mList.Add(new AssetModel() { Title = "Samsung - Galaxy S2", VendorLookup = 8, ModelName = "Galaxy S2", ModelDescription = "PDA for Engineers" });
            mList.Add(new AssetModel() { Title = "UniTrends - Model 6100", VendorLookup = 1, ModelName = "Model 6100", ModelDescription = "Regional Backup System" });
            mList.Add(new AssetModel() { Title = "UniTrends - Model 9100", VendorLookup = 1, ModelName = "Model 9100", ModelDescription = "Corporate Backup System" });

            return mList;

        }

        public List<AssetVendor> GetAssetVendors()
        {
            List<AssetVendor> mList = new List<AssetVendor>();
            // Console.WriteLine("AssetVendors");

            mList.Add(new AssetVendor() { Title = "Apple", VendorName = "Apple", VendorLocation = "Cupertino, CA", VendorPhone = "800-692-7753", VendorEmail = "Tim@apple.com", VendorAddress = "1 Infinite Loop, Cupertino, CA 95014", ContactName = "Tim Cooks" });
            mList.Add(new AssetVendor() { Title = "Barracuda", VendorName = "Barracuda", VendorLocation = "Campbell, CA", VendorPhone = "408-342-5400", VendorEmail = "sales@barracuda.com", VendorAddress = "3175 Winchester Blvd, Campbell, CA 95008", ContactName = "John B" });
            mList.Add(new AssetVendor() { Title = "Cisco", VendorName = "Cisco", VendorLocation = "San Jose, CA", VendorPhone = "800-553-6387", VendorEmail = "sales@cisco.com", VendorAddress = "170 West Tasman Dr, San Jose, CA 95134", ContactName = "Jake C" });
            mList.Add(new AssetVendor() { Title = "Dell", VendorName = "Dell", VendorLocation = "San Jose, CA", VendorPhone = "800-553-6387", VendorEmail = "sales@dell.com", VendorAddress = "170 West Tasman Dr, San Jose, CA 95134", ContactName = "Jacob D" });
            mList.Add(new AssetVendor() { Title = "Fortinet", VendorName = "Fortinet", VendorLocation = "San Jose, CA", VendorPhone = "800-553-6387", VendorEmail = "sales@fortinet.com", VendorAddress = "170 West Tasman Dr, San Jose, CA 95134", ContactName = "Jonathan F" });
            mList.Add(new AssetVendor() { Title = "Google", VendorName = "Google", VendorLocation = "San Jose, CA", VendorPhone = "800-553-6387", VendorEmail = "sales@google.com", VendorAddress = "170 West Tasman Dr, San Jose, CA 95134", ContactName = "Larry Page" });
            mList.Add(new AssetVendor() { Title = "HP", VendorName = "HP", VendorLocation = "San Jose, CA", VendorPhone = "800-553-6387", VendorEmail = "sales@hp.com", VendorAddress = "170 West Tasman Dr, San Jose, CA 95134", ContactName = "Jason H" });
            mList.Add(new AssetVendor() { Title = "IBM", VendorName = "IBM", VendorLocation = "San Jose, CA", VendorPhone = "800-553-6387", VendorEmail = "sales@ibm.com", VendorAddress = "170 West Tasman Dr, San Jose, CA 95134", ContactName = "Jim I" });
            mList.Add(new AssetVendor() { Title = "RIM", VendorName = "RIM", VendorLocation = "San Jose, CA", VendorPhone = "800-553-6387", VendorEmail = "sales@rim.com", VendorAddress = "170 West Tasman Dr, San Jose, CA 95134", ContactName = "Jacob R" });
            mList.Add(new AssetVendor() { Title = "Samsung", VendorName = "Samsung", VendorLocation = "San Jose, CA", VendorPhone = "800-553-6387", VendorEmail = "sales@samsung.com", VendorAddress = "170 West Tasman Dr, San Jose, CA 95134", ContactName = "Job S" });
            mList.Add(new AssetVendor() { Title = "Unitrends", VendorName = "Unitrends", VendorLocation = "San Jose, CA", VendorPhone = "800-553-6387", VendorEmail = "sales@unitrends.com", VendorAddress = "170 West Tasman Dr, San Jose, CA 95134", ContactName = "Jack U" });

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
            mList.Add(new ModuleFormTab() { TabName = "General Information", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General Information" });
            mList.Add(new ModuleFormTab() { TabName = "Related Assets", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Assets" });
            mList.Add(new ModuleFormTab() { TabName = "Tickets", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Tickets" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

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

            // Start from id - 61
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 5;
            string closeStageID = (currStageID + numStages - 2).ToString();
            int moduleStep = 0;

            mList.Add(new LifeCycleStage()
            {
                Name = "Request",
                StageTitle = "Request",
                UserWorkflowStatus = "",
                UserPrompt = "<b>Request or Enter New Asset</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Created",
                ActionUser = "Asset Managers",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Application Created",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Create",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });
            mList.Add(new LifeCycleStage()
            {
                Name = "Acquire",
                StageTitle = "Acquire",
                UserWorkflowStatus = "",
                UserPrompt = "<b>Application is Active</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Asset Acquired",
                ActionUser = "Asset Managers",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Asset Acquired",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Request",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 30,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Active",
                StageTitle = "Active",
                UserWorkflowStatus = "",
                UserPrompt = "<b>Application is Active</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Active",
                ActionUser = "Asset Managers",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Application Retired",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Retire",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 50,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Review/Decommission",
                StageTitle = "Review/Decommission",
                UserWorkflowStatus = "Under review or decomissioning",
                UserPrompt = "<b>Review or Decomission:</b> Retire or return to service",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Review/Decommission",
                ActionUser = "Asset Managers",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Application Retired",
                RejectActionDescription = "",
                ReturnActionDescription = "Re-Activate",
                StageApproveButtonName = "Retire",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 50,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });


            mList.Add(new LifeCycleStage()
            {
                Name = "Retired",
                StageTitle = "Retired",
                UserWorkflowStatus = "",
                UserPrompt = "<b>Retired - Asset is not in use</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Active",
                ActionUser = "Asset Managers",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Retired",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Archive Asset",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Resolved)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Archived",
                StageTitle = "Archived",
                UserWorkflowStatus = "",
                UserPrompt = "<b>Archived - Asset is archived</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Archived",
                ActionUser = "Asset Managers",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 0,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Closed)
            });

            return mList;
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "AssetTagNum", FieldDisplayName = "Asset ID", IsDisplay = true, DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "AssetName", FieldDisplayName = "Asset Name", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "AssetModelLookup", FieldDisplayName = "Asset Model", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "HostName", FieldDisplayName = "Hostname", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "DepartmentLookup", FieldDisplayName = "Department", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "LocationLookup", FieldDisplayName = "Location", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "Status", FieldDisplayName = "Status", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "AssetDescription", FieldDisplayName = "Description", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "TicketId", FieldDisplayName = "Ticket ID", IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "UGITCost", FieldDisplayName = "Cost", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "RequestTypeLookup", FieldDisplayName = "Asset Type", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "VendorLookup", FieldDisplayName = "Vendor", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "HardDrive1", FieldDisplayName = "Hard Drive", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "SerialNum1", FieldDisplayName = "Serial Number", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "PO", FieldDisplayName = "PO", IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "AcquisitionDate", FieldDisplayName = "Acquisition Date", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "PurchasedBy", FieldDisplayName = "Asset Purchased By", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "IPAddress", FieldDisplayName = "IP Address", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "NICAddress", FieldDisplayName = "NIC Address", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "OS", FieldDisplayName = "Operating System", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "AssetDescription", FieldDisplayName = "Asset Description", IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "ReplacementDate", FieldDisplayName = "Replacement Date", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMDB", FieldName = "SupplierChoice", FieldDisplayName = "Supplier", IsUseInWildCard = true, FieldSequence = ++seqNum });
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
            // Tab 0, For New  
            mList.Add(new ModuleFormLayout() { Title = "Asset Inventory", FieldDisplayName = "Asset Inventory", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Asset Name", FieldDisplayName = "Asset Name", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "AssetName", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Asset Tag #", FieldDisplayName = "Asset Tag #", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "AssetTagNum", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = " Asset Disposition", FieldDisplayName = "Asset Disposition", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "AssetDispositionChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Asset Type", FieldDisplayName = "Asset Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Asset Model", FieldDisplayName = "Asset Model", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "AssetModelLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Hostname", FieldDisplayName = "Hostname", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "HostName", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "NIC", FieldDisplayName = "NIC", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "NIC", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "NIC Address", FieldDisplayName = "NIC Address", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "NICName", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "IP Address", FieldDisplayName = "IP Address", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "IPAddress", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });


            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Department", FieldDisplayName = "Department", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "DepartmentLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Location", FieldDisplayName = "Location", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "LocationLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Current User", FieldDisplayName = "Current User", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "CurrentUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Previous User", FieldDisplayName = "Previous User", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PreviousUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "AssetDescription", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "Manufacturer", FieldDisplayName = "Manufacturer", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ManufacturerChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Supplier", FieldDisplayName = "Supplier", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SupplierChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Support #", FieldDisplayName = "Support #", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SupportNumber", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Cost", FieldDisplayName = "Cost", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "Cost", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Acquisition Date", FieldDisplayName = "Acquisition Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "AcquisitionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Retired Date", FieldDisplayName = "Retired Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RetiredDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "PO", FieldDisplayName = "PO", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PO", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Setup Completed Date", FieldDisplayName = "Setup Completed Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SetupCompletedDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Setup Completed By", FieldDisplayName = "Setup Completed By", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SetupCompletedByUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Transfer Date", FieldDisplayName = "Transfer Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "TransferDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Schedule Status", FieldDisplayName = "Schedule Status", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ScheduleStatusChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Sale Date", FieldDisplayName = "Sale Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SaleDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Resold To", FieldDisplayName = "Resold To", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ResoldTo", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Resold For", FieldDisplayName = "Resold For", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ResoldFor", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Resale Value", FieldDisplayName = "Resale Value", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ResaleValue", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Status Change Date", FieldDisplayName = "Status Change Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "StatusChangeDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Registration Date", FieldDisplayName = "Registration Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RegistrationDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Registered By", FieldDisplayName = "Registered By", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RegisteredByUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Data Retention (months)", FieldDisplayName = "Data Retention (months)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "DataRetention", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Additional Key", FieldDisplayName = "Additional Key", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "AdditionalKey", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Asset Inventory", FieldDisplayName = "Asset Inventory", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Software Assets", FieldDisplayName = "Software Assets", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Image Option", FieldDisplayName = "Image Option", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ImageOptionLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Operating System", FieldDisplayName = "Operating System", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OS", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "License Key", FieldDisplayName = "License Key", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "LicenseKey", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "Asset", FieldDisplayName=  "OS Key", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName ="OSKey", ShowInMobile =true, CustomProperties = "" , FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Software", FieldDisplayName = "Software", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "ApplicationMultiLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Software Assets", FieldDisplayName = "Software Assets", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Hardware Asset", FieldDisplayName = "Hardware Assets", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Hard Drive 1", FieldDisplayName = "Hard Drive 1", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "HardDrive1", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Hard Drive 2", FieldDisplayName = "Hard Drive 2", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "HardDrive2", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Image Install Date", FieldDisplayName = "Image Install Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ImageInstallDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Warranty Type", FieldDisplayName = "Warranty Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "WarrantyType", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Warranty Expiration Date", FieldDisplayName = "Warranty Expiration Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "WarrantyExpirationDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "CPU", FieldDisplayName = "CPU", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "CPU", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "RAM (GB)", FieldDisplayName = "RAM (GB)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "RAM", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Hardware Asset", FieldDisplayName = "Hardware Assets", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Secondary Detail", FieldDisplayName = "Secondary Detail", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Serial Num1", FieldDisplayName = "Serial Num1 ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SerialNum1", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Serial Num1 Description", FieldDisplayName = "Serial Num1 Description ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "SerialNum1Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Serial Num2", FieldDisplayName = "Serial Num2 ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SerialNum2", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Serial Num2 Description", FieldDisplayName = "Serial Num2 Description ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "SerialNum2Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Serial Num3 ", FieldDisplayName = "Serial Num3 ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SerialNum3", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Serial Num3 Description", FieldDisplayName = "Serial Num3 Description ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "SerialNum3Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Previous Owner 1", FieldDisplayName = "Previous Owner 1", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PreviousOwner1User", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Previous Owner 2", FieldDisplayName = "Previous Owner 2 ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PreviousOwner2User", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Previous Owner 3", FieldDisplayName = "Previous Owner 3 ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PreviousOwner3User", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "License Type", FieldDisplayName = "License Type ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "LicenseType", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Installed Date", FieldDisplayName = "Installed Date ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "InstalledDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Installed By", FieldDisplayName = "Installed By ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "InstalledByUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Uninstall Date", FieldDisplayName = "Uninstall Date ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "UninstallDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Renewal Date", FieldDisplayName = "Renewal Date ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RenewalDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Upgrade ", FieldDisplayName = "Upgrade ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "UpgradeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Replacement Date", FieldDisplayName = "Replacement Date ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ReplacementDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Actual Replacement Date", FieldDisplayName = "Actual Replacement Date ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ActualReplacementDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Replacement Asset S/N", FieldDisplayName = "Replacement Asset S/N ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ReplacementAsset_SNLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Replacement Delivery Date", FieldDisplayName = "Replacement Delivery Date ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ReplacementDeliveryDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Replacement Ordered Date", FieldDisplayName = "Replacement Ordered Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ReplacementOrderedDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Replacement Type", FieldDisplayName = "Replacement Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ReplacementTypeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments ", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Secondary Detail", FieldDisplayName = "Secondary Detail", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 1: General
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Asset Inventory", FieldDisplayName = "Asset Inventory", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Asset Name", FieldDisplayName = "Asset Name", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "AssetName", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Asset Tag #", FieldDisplayName = "Asset Tag #", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AssetTagNum", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = " Asset Disposition", FieldDisplayName = "Asset Disposition", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AssetDispositionChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Asset Type", FieldDisplayName = "Asset Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Asset Model", FieldDisplayName = "Asset Model", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AssetModelLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Hostname", FieldDisplayName = "Hostname", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "HostName", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "NIC", FieldDisplayName = "NIC", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "NIC", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "NIC Address", FieldDisplayName = "NIC Address", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "NICName", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "IP Address", FieldDisplayName = "IP Address", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "IPAddress", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Department", FieldDisplayName = "Department", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "DepartmentLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Location", FieldDisplayName = "Location", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "LocationLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Current User", FieldDisplayName = "Current User", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "CurrentUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Previous User", FieldDisplayName = "Previous User", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PreviousUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "AssetDescription", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "Manufacturer", FieldDisplayName = "Manufacturer", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ManufacturerChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Supplier", FieldDisplayName = "Supplier", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SupplierChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Support #", FieldDisplayName = "Support #", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SupportNumber", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Cost", FieldDisplayName = "Cost", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Cost", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Acquisition Date", FieldDisplayName = "Acquisition Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AcquisitionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Retired Date", FieldDisplayName = "Retired Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RetiredDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "PO", FieldDisplayName = "PO", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PO", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Setup Completed Date", FieldDisplayName = "Setup Completed Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SetupCompletedDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Setup Completed By", FieldDisplayName = "Setup Completed By", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SetupCompletedByUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Transfer Date", FieldDisplayName = "Transfer Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TransferDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Schedule Status", FieldDisplayName = "Schedule Status", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ScheduleStatusChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Sale Date", FieldDisplayName = "Sale Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SaleDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "ResoldTo", FieldDisplayName = "ResoldTo", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ResoldTo", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Resold For", FieldDisplayName = "Resold For", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ResoldFor", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ResaleValue", FieldDisplayName = "ResaleValue", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ResaleValue", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Status Change Date", FieldDisplayName = "Status Change Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "StatusChangeDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Registration Date", FieldDisplayName = "Registration Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RegistrationDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Registered By", FieldDisplayName = "Registered By", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RegisteredByUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Data Retention (months)", FieldDisplayName = "Data Retention (months)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "DataRetention", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Additional Key", FieldDisplayName = "Additional Key", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AdditionalKey", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Asset Inventory", FieldDisplayName = "Asset Inventory", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Software Assets", FieldDisplayName = "Software Assets", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Image Option", FieldDisplayName = "Image Option", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ImageOptionLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Operating System", FieldDisplayName = "Operating System", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OS", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "License Key", FieldDisplayName = "License Key", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "LicenseKey", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "Asset", FieldDisplayName=  "OS Key", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, "OSKey", ShowInMobile =true, CustomProperties = "" , FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Software", FieldDisplayName = "Software", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ApplicationMultiLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Software Assets", FieldDisplayName = "Software Assets", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Hardware Assets", FieldDisplayName = "Hardware Assets", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Hard Drive 1", FieldDisplayName = "Hard Drive 1", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "HardDrive1", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Hard Drive 2", FieldDisplayName = "Hard Drive 2", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "HardDrive2", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Image Install Date", FieldDisplayName = "Image Install Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ImageInstallDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Warranty Type", FieldDisplayName = "Warranty Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "WarrantyType", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Warranty Expiration Date", FieldDisplayName = "Warranty Expiration Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "WarrantyExpirationDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "CPU", FieldDisplayName = "CPU", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "CPU", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "RAM (GB)", FieldDisplayName = "RAM (GB)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "RAM", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Hardware Assets", FieldDisplayName = "Hardware Assets", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Secondary Detail", FieldDisplayName = "Secondary Detail", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Serial Num1", FieldDisplayName = "Serial Num1 ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SerialNum1", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Serial Num1 Description", FieldDisplayName = "Serial Num1 Description ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "SerialNum1Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Serial Num2 ", FieldDisplayName = "Serial Num2 ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SerialNum2", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Serial Num2 Description", FieldDisplayName = "Serial Num2 Description ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "SerialNum2Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Serial Num3", FieldDisplayName = "Serial Num3 ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SerialNum3", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Serial Num3 Description", FieldDisplayName = "Serial Num3 Description ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "SerialNum3Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Previous Owner 1 ", FieldDisplayName = "Previous Owner 1", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PreviousOwner1User", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Previous Owner 2 ", FieldDisplayName = "Previous Owner 2 ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PreviousOwner2User", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Previous Owner 3 ", FieldDisplayName = "Previous Owner 3 ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PreviousOwner3User", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "LicenseType", FieldDisplayName = "LicenseType ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "LicenseType", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Installed Date ", FieldDisplayName = "Installed Date ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InstalledDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Installed By", FieldDisplayName = "Installed By ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InstalledByUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Uninstall Date", FieldDisplayName = "Uninstall Date ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "UninstallDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Renewal Date", FieldDisplayName = "Renewal Date ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RenewalDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Upgrade", FieldDisplayName = "Upgrade ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "UpgradeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Replacement Date", FieldDisplayName = "Replacement Date ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ReplacementDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Actual Replacement Date", FieldDisplayName = "Actual Replacement Date ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ActualReplacementDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Replacement Asset S/N", FieldDisplayName = "Replacement Asset S/N ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ReplacementAsset_SNLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Replacement Delivery Date", FieldDisplayName = "Replacement Delivery Date ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ReplacementDeliveryDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Replacement Ordered Date", FieldDisplayName = "Replacement Ordered Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ReplacementOrderedDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Replacement Type", FieldDisplayName = "Replacement Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ReplacementTypeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments ", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Secondary Detail", FieldDisplayName = "Secondary Detail", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 2: Related Assets
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Related Assets", FieldDisplayName = "Related Assets", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "AssetRelatedWithAssets", FieldDisplayName = "AssetRelatedWithAssets", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Related Assets", FieldDisplayName = "Related Assets", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 3: Related Incidents
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Related Incidents", FieldDisplayName = "Related Incidents", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "AssetRelatedWithTickets", FieldDisplayName = "AssetRelatedWithTickets", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Related Incidents", FieldDisplayName = "Related Incidents", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 4: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            return mList;


        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            
            mList.Add(new ModuleRequestType() { Title = "Desk Phone", Category = "Phone", RequestType = "Desk Phone", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Desktop", Category = "Computer", RequestType = "Desktop", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Docking Station", Category = "Accessories", RequestType = "Docking Station", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Firewall", Category = "Networking", RequestType = "Firewall", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Keyboard", Category = "Accessories", RequestType = "Keyboard", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Laptop", Category = "Computer", RequestType = "Laptop", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Monitor", Category = "Accessories", RequestType = "Monitor", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Mouse", Category = "Accessories", RequestType = "Mouse", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Printer", Category = "Accessories", RequestType = "Printer", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "SAN", Category = "Data Center", RequestType = "SAN", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Server", Category = "Data Center", RequestType = "Server", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Smartphone", Category = "Phone", RequestType = "Smartphone", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Switch", Category = "Networking", RequestType = "Switch", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "Tablet", Category = "Computer", RequestType = "Tablet", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });
            mList.Add(new ModuleRequestType() { Title = "UPS", Category = "Data Center", RequestType = "UPS", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", PRPGroup = "Asset Managers" });

            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");

            int moduleStep = 0;

            //Editable in all stages
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false });
            //dataList.Add(new string[] { "TicketComment", ModuleName, "0", "0", "0"});

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SerialAssetDetail", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SerialNum1Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SerialNum2Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AssetName", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AssetTagNum", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AssetDispositionChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AssetDescription", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AssetModelLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DepartmentLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LocationLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsDeleted", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DeletedBy", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DeletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "History", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true });
            //mList.Add(new ModuleRoleWriteAccess() { FieldName = "TSRIdLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton=true});
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "StartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "EndDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ModuleStepLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Cost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AcquisitionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RetiredDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AssetsStatusLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PO", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SetupCompletedByUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PreviousOwner1User", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SetupCompletedDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TransferDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ScheduleStatusChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SaleDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResoldTo", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResoldFor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResaleValue", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "StatusChangeDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RegistrationDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RegisteredByUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PreviousUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AdditionalKey", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LicenseKey", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OSKey", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImageOptionLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ApplicationMultiLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OS", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "HardDrive1", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "HardDrive2", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImageInstallDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "WarrantyType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "WarrantyExpirationDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CPU", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RAM", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SerialNum1", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SerialNum2", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SerialNum3", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SerialNum3Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PreviousOwner2User", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PreviousOwner3User", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LicenseType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "InstalledDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "InstalledByUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UninstallDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RenewalDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UpgradeChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReplacementDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ActualReplacementDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReplacementAsset_SNLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReplacementDeliveryDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReplacementOrderedDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReplacementTypeChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ManufacturerChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SupplierChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "HostName", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SupportNumber", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DataRetention", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true });


            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            return mList;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {

        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "New", ViewName = "CMDB", TabOrder = 1, ModuleNameLookup = "CMDB", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "CMDB", TabOrder = 2, ModuleNameLookup = "CMDB", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "CMDB", TabOrder = 3, ModuleNameLookup = "CMDB", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "CMDB", TabOrder = 4, ModuleNameLookup = "CMDB", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "CMDB", TabOrder = 5, ModuleNameLookup = "CMDB", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "Active", ViewName = "CMDB", TabOrder = 6, ModuleNameLookup = "CMDB", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "Retired", ViewName = "CMDB", TabOrder = 7, ModuleNameLookup = "CMDB", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "CMDB", TabOrder = 8, ModuleNameLookup = "CMDB", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "Archived", ViewName = "CMDB", TabOrder = 9, ModuleNameLookup = "CMDB", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "CMDB", TabOrder = 10, ModuleNameLookup = "CMDB", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });
            return tabs;
        }
    }
}
