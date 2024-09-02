using System;
using System.Collections.Generic;
using System.Data;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ModuleColumnHelper
    {
        ApplicationContext _spWeb;
        public const String ViewFields = @"<FieldRef Name='ID' Nullable='TRUE'/><FieldRef Name='ModuleNameLookup' Nullable='TRUE'/><FieldRef Name='FieldSequence' Nullable='TRUE'/><FieldRef Name='FieldName' Nullable='TRUE'/><FieldRef Name='FieldDisplayName' Nullable='TRUE'/>
                                           <FieldRef Name='SortOrder' Nullable='TRUE'/><FieldRef Name='IsDisplay' Nullable='TRUE'/><FieldRef Name='ShowInMobile' Nullable='TRUE'/>
                                           <FieldRef Name='DisplayForClosed' Nullable='TRUE'/><FieldRef Name='IsUseInWildCard' Nullable='TRUE'/><FieldRef Name='DisplayForReport' Nullable='TRUE'/>
                                           <FieldRef Name='CustomProperties' Nullable='TRUE'/><FieldRef Name='ColumnType' Nullable='TRUE'/><FieldRef Name='IsAscending' Nullable='TRUE'/>";

        public ModuleColumnHelper(ApplicationContext spWeb)
        {
            _spWeb = spWeb;
        }

        public List<ModuleColumn> LoadByModule(string moduleName)
        {
            List<ModuleColumn> objs = new List<ModuleColumn>();

            //SPQuery query = new SPQuery();
            //query.ViewFields = ViewFields;
            //query.ViewFieldsOnly = true;
            string query = string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, moduleName);
            DataRow[] collection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, query).Select();

            foreach (DataRow item in collection)
            {
                ModuleColumn column = LoadItem(item);
                if (!objs.Exists(x => x.FieldName == column.FieldName))
                    objs.Add(column);
            }

            return objs;
        }

        private ModuleColumn LoadItem(DataRow item)
        {
            ModuleColumn m = new ModuleColumn();

            m.ID =Convert.ToInt32(item["ID"]);

            string moduleLookup =Convert.ToString(item[DatabaseObjects.Columns.ModuleNameLookup]);
            m.CategoryName = moduleLookup;
            m.FieldDisplayName = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.FieldDisplayName));
            m.FieldName = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.FieldName));
            m.FieldSequence = UGITUtility.StringToInt(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.FieldSequence));
            m.ShowInMobile = UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.ShowInMobile));
            m.IsUseInWildCard = UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.IsUseInWildCard));
            m.IsDisplay = UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.IsDisplay));
            m.DisplayForClosed = UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.DisplayForClosed));
            m.DisplayForReport = UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.DisplayForReport));
            m.ColumnType = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.ColumnType));
            m.CustomProperties = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.CustomProperties));
            Dictionary<string, string> customProperties = UGITUtility.GetCustomProperties(m.CustomProperties, Constants.Separator);

            if (customProperties.ContainsKey(CustomProperties.UseforGlobalDateFilter)) {
                // m.Prop_UseforGlobalDateFilter = UGITUtility.StringToBoolean(customProperties[CustomProperties.UseforGlobalDateFilter]);
            }
            if (customProperties.ContainsKey(CustomProperties.MiniView)) {
                // m.Prop_MiniView = UGITUtility.StringToBoolean(customProperties[CustomProperties.MiniView]);
            }
            return m;
        }

        /// <summary>
        /// Add default columns that are required for correct operation of system
        /// </summary>
        /// <param name="module"></param>
        /// <param name="columns"></param>
        public void AddDefaultColumns(UGITModule module, List<ModuleColumn> columns)
        {
            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.Id))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.Id;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = DatabaseObjects.Columns.Id;
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketId))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketId;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Ticket ID";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.StageStep))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.StageStep;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Stage Step";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.ModuleStepLookup))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.ModuleStepLookup;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Module Step Name";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            // For SMS modules, add Initiator & Requestor
            if (module.ModuleType  == ModuleType.SMS)
            {
                if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketInitiator))
                {
                    ModuleColumn column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.TicketInitiator;
                    column.CategoryName = module.ModuleName;
                    column.FieldDisplayName = "Initiator";
                    column.FieldSequence = columns.Count + 1;
                    columns.Add(column);
                }
                if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketRequestor))
                {
                    ModuleColumn column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.TicketRequestor;
                    column.CategoryName = module.ModuleName;
                    column.FieldDisplayName = "Requestor";
                    column.ColumnType = "MultiUser";
                    column.FieldSequence = columns.Count + 1;
                    columns.Add(column);
                }
            }
            if (module.ModuleType == ModuleType.SMS && !columns.Exists(x => x.FieldName == DatabaseObjects.Columns.ApprovedBy))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.ApprovedBy;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Approved By";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (module.ModuleType == ModuleType.SMS && !columns.Exists(x => x.FieldName == DatabaseObjects.Columns.ApprovalDate))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.ApprovalDate;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Approval Date";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }
            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketOnHold))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketOnHold;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "On Hold";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (module.ModuleName == "NPR" && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketPMMIdLookup))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketPMMIdLookup;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "PMM ID";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if ((module.ModuleName == "NPR" || module.ModuleName == "PMM") && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketActualStartDate))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketActualStartDate;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Start Date";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            // These three needed for GovernanceReview control
            if ((module.ModuleName == "NPR" || module.ModuleName == "PMM") && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketActualCompletionDate;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Due Date";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if ((module.ModuleName == "NPR" || module.ModuleName == "PMM" || module.ModuleType  == ModuleType.SMS) && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketPriorityLookup))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketPriorityLookup;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Priority";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if ((module.ModuleName == "NPR" || module.ModuleName == "PMM") && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketSponsors))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketSponsors;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Sponsors";
                column.ColumnType = "MultiUser";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if ((module.ModuleName == "NPR" || module.ModuleName == "PMM") && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.ProjectInitiativeLookup))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.ProjectInitiativeLookup;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Initiative";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if ((module.ModuleName == "ACR" || module.ModuleName == "BTS" || module.ModuleName == "PRS" || module.ModuleName == "TSR") &&
                (module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.SelfAssign) && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.PRPGroup)))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.PRPGroup;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "PRP Group";
                column.ColumnType = "MultiUser";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketClosed))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketClosed;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Closed";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketCloseDate))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketCloseDate;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Closed On";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.DepartmentLookup))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.DepartmentLookup;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = uHelper.GetDepartmentLabelName(_spWeb, DepartmentLevel.Department);
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.DivisionLookup))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.DivisionLookup;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = uHelper.GetDepartmentLabelName(_spWeb, DepartmentLevel.Division);
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.CompanyTitleLookup))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.CompanyTitleLookup;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = uHelper.GetDepartmentLabelName(_spWeb, DepartmentLevel.Company);
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.IsPrivate))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.IsPrivate;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Private";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketPRP))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketPRP;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "PRP";
                column.IsDisplay = false;
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketORP))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketORP;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "ORP";
                column.IsDisplay = false;
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if ((module.ModuleName == "NPR" || module.ModuleName == "PMM" || module.ModuleType  == ModuleType.SMS) && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketStageActionUserTypes))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketStageActionUserTypes;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Action User Types";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if ((module.ModuleName == "NPR" || module.ModuleName == "PMM" || module.ModuleType == ModuleType.SMS) && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketStageActionUsers))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketStageActionUsers;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Action Users";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if ((module.ModuleName == "NPR" || module.ModuleName == "PMM" || module.ModuleName == "TSK") && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketBeneficiaries))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.TicketBeneficiaries;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Primary Beneficiaries";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (module.ModuleName == "CMDB" && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.AssetTagNum))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.AssetTagNum;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Asset Tag";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (module.ModuleName == "CMDB" && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.CurrentUser))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.CurrentUser;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Current User";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (module.ModuleName == "INC" && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.OutageHours))
            {
                ModuleColumn column = new ModuleColumn();
                column.FieldName = DatabaseObjects.Columns.OutageHours;
                column.CategoryName = module.ModuleName;
                column.FieldDisplayName = "Outage Hours";
                column.FieldSequence = columns.Count + 1;
                columns.Add(column);
            }

            if (module.ModuleName == "VSW" || module.ModuleName == "VSL" || module.ModuleName == "VFM" || module.ModuleName == "VPM" || module.ModuleName == "VCC")
            {
                if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.VendorMSALookup))
                {
                    ModuleColumn column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.VendorMSALookup;
                    column.CategoryName = module.ModuleName;
                    column.FieldDisplayName = "MSA ID";
                    column.FieldSequence = columns.Count + 1;
                    columns.Add(column);
                }

                if (module.ModuleName != "VCC" && !module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.VendorMSANameLookup))
                {
                    ModuleColumn column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.VendorMSANameLookup;
                    column.CategoryName = module.ModuleName;
                    column.FieldDisplayName = "MSA";
                    column.FieldSequence = columns.Count + 1;
                    columns.Add(column);
                }
            }

            if (module.ModuleName == "VSL" || module.ModuleName == "VFM" || module.ModuleName == "VPM" || module.ModuleName == "VCC")
            {
                if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.VendorSOWLookup))
                {
                    ModuleColumn column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.VendorSOWLookup;
                    column.CategoryName = module.ModuleName;
                    column.FieldDisplayName = "SOW ID";
                    column.FieldSequence = columns.Count + 1;
                    columns.Add(column);
                }

                if (!module.List_ModuleColumns.Exists(x => x.FieldName == DatabaseObjects.Columns.VendorSOWNameLookup))
                {
                    ModuleColumn column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.VendorSOWNameLookup;
                    column.CategoryName = module.ModuleName;
                    column.FieldDisplayName = "SOW";
                    column.FieldSequence = columns.Count + 1;
                    columns.Add(column);
                }
            }
            
            module.List_ModuleColumns = columns;
        }
    }
}
