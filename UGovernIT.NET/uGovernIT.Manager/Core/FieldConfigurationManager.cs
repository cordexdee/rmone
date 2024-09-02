using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;


namespace uGovernIT.Manager
{
    public interface IFieldConfigurationManager : IManagerBase<FieldConfiguration>
    {
        DataTable GetFieldDataByFieldName(string fieldName, string ModuleName);
        DataTable GetFieldDataByFieldName(string fieldName);

        FieldConfiguration GetFieldByFieldName(string fieldName);
        List<FieldConfiguration> GetFieldConfigurationData();

        string GetFieldConfigurationData(string fieldName, string values);
        string GetFieldConfigurationIdByName(string fieldName, string values);
    }

    public class FieldConfigurationManager : ManagerBase<FieldConfiguration>
    {
        ModuleViewManager _moduleViewManager;
        string values = string.Empty;
        public FieldConfigurationManager(ApplicationContext context) : base(context)
        {
            store = new FieldConfigurationStore(this.dbContext);
            _moduleViewManager = new ModuleViewManager(this.dbContext);
        }

        /// <summary>
        /// It will handle only Lookup & Choice type fields
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="mappedTable"></param>
        /// <returns></returns>
        public FieldConfiguration GetFieldByFieldName(string fieldName, string mappedTable = "")
        {
            FieldConfiguration field = null;
            //It will handle only Lookup & Choice type fields
            //List<string> lstOfAllowedFields = new List<string>() { "Lookup", "Choice", "UserGroup", "User", "ModuleType", "MultiLookup" };
            //if (!lstOfAllowedFields.Any(x => fieldName.EndsWith(x)))
            //    return field;
            //string key = fieldName;
            //bool tableExist = false;
            //if (!string.IsNullOrEmpty(mappedTable) && fieldName.EndsWith("Choice"))
            //    tableExist = true;

            // Above code commented by Anurag because this is not working all fields datatype, we need to get datatype of all field if exist in FieldConfiguration table.
            // This will help use display data on modulewebpart.
            bool tableExist = false;
            if (!string.IsNullOrEmpty(mappedTable))
                tableExist = true;
            if (tableExist)
                field = CacheHelper<FieldConfiguration>.Get(string.Format("{0}_{1}", fieldName, mappedTable), this.dbContext.TenantID);

            if (field == null)
                field = CacheHelper<FieldConfiguration>.Get(fieldName, this.dbContext.TenantID);

            if (field == null)
            {
                if (tableExist)
                    field = store.Get(x => x.FieldName == fieldName && x.TableName == mappedTable);

                if (field == null)
                    field = store.Get(x => x.FieldName == fieldName);

                if (field != null)
                {
                    if (fieldName.EndsWith("Choice") && !string.IsNullOrEmpty(field.TableName))
                        CacheHelper<FieldConfiguration>.AddOrUpdate(string.Format("{0}_{1}", fieldName, field.TableName), this.dbContext.TenantID, field);
                    else
                        CacheHelper<FieldConfiguration>.AddOrUpdate(fieldName, this.dbContext.TenantID, field);
                }
            }
            return field;
        }

        public DataTable GetFieldDataByFieldName(string fieldName)
        {
            return GetFieldDataByFieldName(fieldName, string.Empty, null);
        }

        public DataTable GetFieldDataByFieldName(string fieldName,string tableName)
        {
          return GetFieldDataByFieldName(fieldName, tableName, null);
        }
        public DataTable GetFieldDataByFieldName(string fieldName, string mappedTable = "",TicketColumnError columnErrors=null)
        {
            DataTable dt = new DataTable();
            FieldConfiguration field = GetFieldByFieldName(fieldName, mappedTable);
            if (field != null)
            {
                FieldType fType = FieldType.None;
                Enum.TryParse(field.Datatype, out fType);
                switch (fType)
                {
                    case FieldType.UserField:
                        {
                            dt = UGITUtility.ToDataTable<UserProfile>(dbContext.UserManager.GetUsersProfileWithGroup(dbContext.TenantID));
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                dt = dt.DefaultView.ToTable(true, "Id", DatabaseObjects.Columns.UserName, DatabaseObjects.Columns.Name, DatabaseObjects.Columns.TenantID);
                                dt.Columns[DatabaseObjects.Columns.UserName].ColumnName = DatabaseObjects.Columns.Title;
                                dt.Columns[DatabaseObjects.Columns.Id].ColumnName = DatabaseObjects.Columns.ID;
                            }
                        }
                        break;
                    case FieldType.Lookup:
                        ResolveLookupValues(field.ParentTableName, ref dt);                                            

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketId, dt))
                                dt = dt.DefaultView.ToTable(true, DatabaseObjects.Columns.Id, field.ParentFieldName, DatabaseObjects.Columns.TenantID, DatabaseObjects.Columns.TicketId);
                            else
                                dt = dt.DefaultView.ToTable(true, DatabaseObjects.Columns.Id, field.ParentFieldName, DatabaseObjects.Columns.TenantID);

                            dt.Columns[field.ParentFieldName].ColumnName = DatabaseObjects.Columns.Title;
                            dt.Columns[DatabaseObjects.Columns.Id].ColumnName = DatabaseObjects.Columns.ID;
                        }
                        break;
                    case FieldType.Choices:
                        string[] dataRequestSource = UGITUtility.SplitString(field.Data, Constants.Separator);
                        dt.Columns.Add(DatabaseObjects.Columns.ID);
                        dt.Columns.Add(DatabaseObjects.Columns.Title);
                        foreach (string choice in dataRequestSource)
                        {
                            dt.Rows.Add(choice, choice);
                        }
                        break;
                }
            }
            else if (columnErrors != null)
                columnErrors= TicketColumnError.AddError(fieldName, string.Empty, "Field not found in Field Configuration");

            return dt;
        }

        //changes by mayank singh //
        public DataTable GetFieldDataByFieldName(string fieldName, string ModuleName, string FilterExpression = null, string TenantID = null)
        {
            string conditon = DatabaseObjects.Columns.ModuleNameLookup + "='" + ModuleName + "'";
            DataTable dt = new DataTable();
            FieldConfiguration field = null;
            if (fieldName.EndsWith("Choice"))
            {
                string moduleTable = _moduleViewManager.GetModuleTableName(ModuleName);
                if (!string.IsNullOrEmpty(moduleTable))
                    field = this.GetFieldByFieldName(fieldName, moduleTable);
                else
                    field = this.GetFieldByFieldName(fieldName);
            }
            else
            {
                field = this.GetFieldByFieldName(fieldName);
            }

            if (field != null)
            {
                TenantID = field.TenantID;
                if (!string.IsNullOrEmpty(TenantID))
                {
                    conditon = DatabaseObjects.Columns.ModuleNameLookup + "='" + ModuleName + "'" + " and " + DatabaseObjects.Columns.TenantID + "='" + TenantID + "' and " + DatabaseObjects.Columns.Deleted + " = 'False'";
                }

                FieldType fType = FieldType.None;
                Enum.TryParse(field.Datatype, out fType);
                switch (fType)
                {
                    case FieldType.UserField:
                        dt = UGITUtility.ToDataTable<UserProfile>(dbContext.UserManager.GetUsersProfile());
                        if (dt != null && !string.IsNullOrEmpty(FilterExpression))
                        {
                            DataRow[] dr = dt.Select(FilterExpression);
                            if (dr.Count() > 0)
                                dt = dr.CopyToDataTable();
                            else
                                dt = dt.Clone();
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            dt = dt.DefaultView.ToTable(true, "Id", DatabaseObjects.Columns.UserName, DatabaseObjects.Columns.Name);
                            dt.Columns[DatabaseObjects.Columns.UserName].ColumnName = DatabaseObjects.Columns.Title;
                            dt.Columns[DatabaseObjects.Columns.Id].ColumnName = DatabaseObjects.Columns.ID;
                        }

                        break;
                    case FieldType.Lookup:
                        if (!string.IsNullOrEmpty(ModuleName) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.ModuleNameLookup, field.ParentTableName))
                        {
                            if (field.ParentTableName == DatabaseObjects.Tables.RequestType)
                            {
                                ModuleViewManager moduleViewManager = new ModuleViewManager(dbContext);
                                dt = UGITUtility.ToDataTable<ModuleRequestType>(moduleViewManager.LoadByName(ModuleName).List_RequestTypes);
                            }
                            else
                                dt = GetTableDataManager.GetTableData(field.ParentTableName, conditon);
                        }
                        else
                        {
                            conditon = DatabaseObjects.Columns.TenantID + "='" + TenantID + "'" + " and " + DatabaseObjects.Columns.Deleted + "='False'";
                            string cacheName = "Lookup_" + field.ParentTableName + "_" + dbContext.TenantID;
                            if (CacheHelper<object>.IsExists(cacheName, dbContext.TenantID))
                            {
                                dt = CacheHelper<object>.Get(cacheName, dbContext.TenantID) as DataTable;
                                if (dt == null)
                                    dt = GetTableDataManager.GetTableData(field.ParentTableName, conditon);

                                if(field.FieldName == DatabaseObjects.Columns.StudioLookup)
                                {
                                    dt.DefaultView.Sort = field.ParentFieldName;
                                }
                                //Added since Tables like ACRTypes doesnot have ModuleNameLookup, but needs to be filtered by Tenant.
                                //dt = GetTableDataManager.GetTableData(field.ParentTableName);
                                if (dt.Columns.Contains(DatabaseObjects.Columns.Deleted))
                                {
                                    DataView dv = dt.DefaultView;
                                    dv.RowFilter = string.Format("{0}<>True", DatabaseObjects.Columns.Deleted);
                                    dt = dv.ToTable();
                                }
                            }
                            else
                            {
                                dt = GetTableDataManager.GetTableData(field.ParentTableName, conditon);
                                CacheHelper<object>.AddOrUpdate(cacheName, dbContext.TenantID, dt.Copy());
                            }

                            if (field.ParentTableName.ToLower() == DatabaseObjects.Tables.ProjectClass.ToLower())
                            {
                                DataView dv = dt.DefaultView;
                                if (dt.Columns.Contains(DatabaseObjects.Columns.Deleted))
                                {
                                    dv.RowFilter = string.Format("{0}<>1", DatabaseObjects.Columns.Deleted);
                                    dt = dv.ToTable();
                                }


                            }
                        }
                        if (dt != null && dt.Rows.Count > 0 && !string.IsNullOrEmpty(FilterExpression))
                        {
                            DataRow[] dr = dt.Select(FilterExpression);
                            if (dr.Count() > 0)
                                dt = dr.CopyToDataTable();
                            else
                                dt = dt.Clone();
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (field.ParentTableName == DatabaseObjects.Tables.RequestType)
                            {
                                dt = dt.DefaultView.ToTable(true, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Category, DatabaseObjects.Columns.SubCategory, DatabaseObjects.Columns.RequestType, DatabaseObjects.Columns.Title);
                                //dt.Columns[DatabaseObjects.Columns.TicketRequestType].ColumnName = DatabaseObjects.Columns.Title;
                            }
                            else if (field.ParentTableName == DatabaseObjects.Tables.Assets)
                            {
                                DataTable newTable = dt.Clone();
                                newTable.Columns[DatabaseObjects.Columns.AssetModelLookup].DataType = typeof(string);
                                newTable.Columns[DatabaseObjects.Columns.TicketRequestTypeLookup].DataType = typeof(string);
                                foreach (DataRow row in dt.Rows)
                                {
                                    newTable.ImportRow(row);
                                }
                                for (int i = 0; i < newTable.Rows.Count; i++)
                                {
                                    values = GetFieldConfigurationData(DatabaseObjects.Columns.AssetModelLookup, Convert.ToString(newTable.Rows[i][DatabaseObjects.Columns.AssetModelLookup]), null);
                                    if (!string.IsNullOrWhiteSpace(values))
                                    {

                                        newTable.Rows[i][DatabaseObjects.Columns.AssetModelLookup] = values.Replace(',', '>');
                                    }
                                    else
                                    {
                                        newTable.Rows[i][DatabaseObjects.Columns.AssetModelLookup] = String.Empty;
                                    }
                                    values = GetFieldConfigurationData(DatabaseObjects.Columns.TicketRequestTypeLookup, Convert.ToString(newTable.Rows[i][DatabaseObjects.Columns.TicketRequestTypeLookup]), null);
                                    if (!string.IsNullOrWhiteSpace(values))
                                    {

                                        newTable.Rows[i][DatabaseObjects.Columns.TicketRequestTypeLookup] = values.Replace(',', '>');
                                    }
                                    else
                                    {
                                        newTable.Rows[i][DatabaseObjects.Columns.TicketRequestTypeLookup] = String.Empty;
                                    }
                                }
                                dt = newTable;
                                //dt = dt.DefaultView.ToTable(true, DatabaseObjects.Columns.Id, field.ParentFieldName, DatabaseObjects.Columns.AssetName, DatabaseObjects.Columns.AssetTagNum, "Status", DatabaseObjects.Columns.AssetModelLookup, DatabaseObjects.Columns.HostName, DatabaseObjects.Columns.Owner, DatabaseObjects.Columns.TicketRequestTypeLookup, DatabaseObjects.Columns.CurrentUser);
                                //dt.Columns[DatabaseObjects.Columns.Id].ColumnName = DatabaseObjects.Columns.ID;
                                //dt.Columns[field.ParentFieldName].ColumnName = DatabaseObjects.Columns.Title;

                            }
                            else if (field.ParentTableName.Equals(DatabaseObjects.Tables.AspNetRoles, StringComparison.InvariantCultureIgnoreCase))
                            {
                                UserProfile user = this.dbContext.CurrentUser;
                                bool isSuperAdmin = dbContext.UserManager.IsUGITSuperAdmin(user);
                                if (!isSuperAdmin)
                                {
                                    dt = dt.AsEnumerable().Where(row => row.Field<String>(DatabaseObjects.Columns.Name) != "UGITSuperAdmin").CopyToDataTable();
                                }
                                dt = dt.DefaultView.ToTable(true, DatabaseObjects.Columns.Id, field.ParentFieldName);
                                dt.Columns[DatabaseObjects.Columns.Id].ColumnName = DatabaseObjects.Columns.ID;
                                dt.Columns[field.ParentFieldName].ColumnName = DatabaseObjects.Columns.Title;
                            }
                            
                            else if (field.ParentTableName == DatabaseObjects.Tables.AssetModels)
                            {
                                DataTable newTable = dt.Clone();
                                newTable.Columns[DatabaseObjects.Columns.VendorLookup].DataType = typeof(string);
                                foreach (DataRow row in dt.Rows)
                                {
                                    newTable.ImportRow(row);
                                }
                                for (int i = 0; i < newTable.Rows.Count; i++)
                                {
                                        values = GetFieldConfigurationData(DatabaseObjects.Columns.VendorLookup, Convert.ToString(newTable.Rows[i][DatabaseObjects.Columns.VendorLookup]), null);
                                    if (!string.IsNullOrWhiteSpace(values))
                                    {

                                        newTable.Rows[i][DatabaseObjects.Columns.VendorLookup] = values.Replace(',', '>');
                                    }
                                    else
                                    {
                                        newTable.Rows[i][DatabaseObjects.Columns.VendorLookup] = String.Empty;
                                    }
                                }
                                dt = newTable;//ToTable(true, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title,DatabaseObjects.Columns.VendorLookup);
                            }
                            else if (field.ParentTableName == DatabaseObjects.Tables.CRMContact || field.ParentTableName == DatabaseObjects.Tables.CRMCompany)
                            {
                                dt = dt.DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId, field.ParentFieldName);
                                dt.Columns[DatabaseObjects.Columns.TicketId].ColumnName = DatabaseObjects.Columns.TicketId;
                                dt.Columns[field.ParentFieldName].ColumnName = DatabaseObjects.Columns.Title;
                            }
                            else
                            {
                                dt = dt.DefaultView.ToTable(true, DatabaseObjects.Columns.ID, field.ParentFieldName);
                                //dt.Columns[DatabaseObjects.Columns.Id].ColumnName = DatabaseObjects.Columns.ID;
                                //dt.Columns[field.ParentFieldName].ColumnName = DatabaseObjects.Columns.Title;
                            }


                            }
                            break;
                    case FieldType.Choices:
                        string[] dataRequestSource = UGITUtility.SplitString(field.Data, Constants.Separator);
                        dt.Columns.Add(DatabaseObjects.Columns.ID);
                        dt.Columns.Add(DatabaseObjects.Columns.Title);
                        foreach (string choice in dataRequestSource)
                        {
                            dt.Rows.Add(choice, choice);
                        }
                        break;
                }
            }
            return dt;
        }

        public string GetFieldConfigurationData(FieldConfiguration field, string values, List<UserProfile> userProfiles = null, string seperator = ";#", ModuleColumn moduleColumn = null)
        {
            //if (field == null)
            //    return values;

            DataTable dt = new DataTable();
            var outputValue = string.Empty;
            FieldType fType = FieldType.None;
            string pTableName = string.Empty;
            string pFieldName = string.Empty;
            Guid GuidValue = Guid.Empty;
            long longValue = 0;
            if (field != null)
            {
                Enum.TryParse(field.Datatype, out fType);
                pTableName = field.ParentTableName;
                pFieldName = field.ParentFieldName;
            }
            else if (moduleColumn != null)
                Enum.TryParse(moduleColumn.ColumnType, out fType);

            if (string.IsNullOrEmpty(Convert.ToString(fType)))
                return values;

            switch (fType)
            {
                case FieldType.UserField:
                    if (!string.IsNullOrEmpty(values) && values.Contains(Constants.Separator6))
                        seperator = Constants.Separator6;

                    var userList = UGITUtility.ConvertStringToList(values, seperator);
                    string commanames = dbContext.UserManager.CommaSeparatedNamesFrom(userList, seperator, userProfiles);
                    outputValue = commanames;
                    break;

                case FieldType.GroupField:
                    List<string> grouplist = UGITUtility.ConvertStringToList(values, Constants.Separator6);
                    string commagroups = dbContext.UserManager.CommaSeparatedGroupNamesFrom(grouplist, Constants.Separator);
                    outputValue = commagroups;
                    break;

                case FieldType.Date:
                    outputValue = UGITUtility.GetDateStringInFormat(values, false);
                    break;

                case FieldType.Lookup:
                    if (!string.IsNullOrEmpty(values))
                    {
                        List<string> lookupIds = UGITUtility.ConvertStringToList(values, Constants.Separator6);
                        List<string> listLookupValues = new List<string>();


                        string cacheName = "Lookup_" + pTableName + "_" + dbContext.TenantID;

                        if (CacheHelper<object>.IsExists(cacheName, dbContext.TenantID))
                            dt = CacheHelper<object>.Get(cacheName, dbContext.TenantID) as DataTable;
                        else
                        {
                            dt = GetTableDataManager.GetTableData(pTableName, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
                            CacheHelper<object>.AddOrUpdate(cacheName, dbContext.TenantID, dt);
                        }


                        ResolveLookupValues(pTableName, ref dt);

                        lookupIds.ForEach(x =>
                        {
                            DataRow[] dataRow = null;
                            if (!string.IsNullOrEmpty(pTableName) && pTableName.Equals(DatabaseObjects.Tables.Modules))
                                dataRow = dt.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleName, x));
                            else
                            {
                                if (long.TryParse(x, out longValue))
                                    dataRow = dt.Select("ID=" + longValue);
                                else if (Guid.TryParse(x, out GuidValue))
                                    dataRow = dt.Select($"ID='{GuidValue}'");
                                else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketId, dt))
                                    dataRow = dt.Select($"{DatabaseObjects.Columns.TicketId}='{x}'");
                            }
                            if (dataRow != null && dataRow.Count() > 0)
                            {
                                listLookupValues.Add(Convert.ToString(dataRow[0][pFieldName]));
                            }
                        });

                        outputValue = string.Join(Constants.Separator6, listLookupValues.ToArray());
                    }
                    break;

                case FieldType.Percentage:
                    if (!string.IsNullOrEmpty(values))
                    {
                        outputValue = values + " %";
                    }

                    break;

                case FieldType.Currency:
                    if (!string.IsNullOrEmpty(values))
                    {
                        CultureInfo ci = new CultureInfo("en-US");
                        var symbol = ci.NumberFormat.CurrencySymbol;
                        outputValue = symbol + values;
                    }
                    break;

                case FieldType.Attachments:
                    if (!string.IsNullOrEmpty(values))
                    {
                        var data = GetTableDataManager.GetTableData(pTableName, $"FileID='{values}'").AsEnumerable().FirstOrDefault();
                        if (data != null)
                        {
                            outputValue = data.Field<string>("Name");
                        }
                    }
                    break;

                default:
                    outputValue = values;
                    break;
            }
            return outputValue;
        }

        public string GetFieldConfigurationData(string fieldName, string values, ModuleColumn moduleColumn = null)
        {
            FieldConfiguration field = GetFieldByFieldName(fieldName);
            //if (field == null)
            //    return values;
            DataTable dt = new DataTable();
            string outputValue = string.Empty;
            Guid GuidValue = Guid.Empty;
            long longValue = 0;
            bool result = true;
            FieldType fType = FieldType.None;
            string pTableName = string.Empty;
            string pFieldName = string.Empty;
            if (field != null)
            {
                Enum.TryParse(field.Datatype, out fType);
                pTableName = field.ParentTableName;
                pFieldName = field.ParentFieldName;
                if (field.Datatype.EqualsIgnoreCase("datetime"))
                    fType = FieldType.DateTime;
                    
            }
             
            else if (moduleColumn != null)
                Enum.TryParse(moduleColumn.ColumnType, out fType);

            if (string.IsNullOrEmpty(Convert.ToString(fType)))
                return values;

            switch (fType)
            {
                case FieldType.MultiUser:
                case FieldType.UserField:
                    List<string> userlist = new List<string>();
                    if (values.Contains(Constants.Separator))
                        userlist = UGITUtility.ConvertStringToList(values, Constants.Separator);
                    else
                        userlist = UGITUtility.ConvertStringToList(values, Constants.Separator6);

                    if (userlist.Count > 0)
                    {
                        result = Guid.TryParse(userlist[0], out GuidValue);
                    }
                    if (result)
                    {
                        string commanames = dbContext.UserManager.CommaSeparatedNamesFrom(userlist, Constants.UserInfoSeparator);
                        if (string.IsNullOrEmpty(commanames))
                        {
                            commanames = dbContext.UserManager.CommaSeparatedGroupNamesFrom(userlist, Constants.UserInfoSeparator);
                        }
                        outputValue = commanames;
                    }
                    else
                    {
                        outputValue = values;
                    }
                    break;
                case FieldType.GroupField:
                    List<string> grouplist = UGITUtility.ConvertStringToList(values, Constants.Separator6);
                    string commagroups = dbContext.UserManager.CommaSeparatedGroupNamesFrom(grouplist, Constants.Separator);
                    outputValue = commagroups;
                    break;
                case FieldType.Date:
                    outputValue = UGITUtility.GetDateStringInFormat(values, false);
                    break;
                case FieldType.DateTime:
                    outputValue = UGITUtility.GetDateStringInFormat(values, true);
                    break;
                case FieldType.Lookup:
                    if (!string.IsNullOrEmpty(values) && !values.Equals("[No Value]"))
                    {
                        List<string> lookupIds = UGITUtility.ConvertStringToList(values, Constants.Separator6);
                        if(values.Contains(Constants.Separator))
                            lookupIds = UGITUtility.ConvertStringToList(values, Constants.Separator);
                        if (lookupIds.Count > 0)
                        {
                            result = long.TryParse(lookupIds[0], out longValue) || Guid.TryParse(lookupIds[0], out GuidValue);
                        }

                        if (result)
                        {
                            List<string> listLookupValues = new List<string>();
                            string cacheName = "Lookup_" + pTableName + "_" + dbContext.TenantID;
                            if (CacheHelper<object>.IsExists(cacheName, dbContext.TenantID))
                            {
                                dt = CacheHelper<object>.Get(cacheName, dbContext.TenantID) as DataTable;
                                if (dt == null)
                                {
                                    dt = GetTableDataManager.GetTableData(pTableName, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
                                    CacheHelper<object>.AddOrUpdate(cacheName, dbContext.TenantID, dt);
                                }
                            }
                            else
                            {
                                dt = GetTableDataManager.GetTableData(pTableName, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
                                CacheHelper<object>.AddOrUpdate(cacheName, dbContext.TenantID, dt);
                            }
                            lookupIds.ForEach(x =>
                            {
                                DataRow[] dataRow = null;
                                if (field.ParentTableName != null && pTableName.Equals(DatabaseObjects.Tables.Modules))
                                    dataRow = dt.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleName, x));
                                else
                                {
                                    //dataRow = dt.Select("ID=" + x);
                                    if (long.TryParse(x, out longValue))
                                    {
                                        dataRow = dt.Select("ID=" + longValue);
                                        //Added below condition, when new record is added in Transaction table like Opportunity, but could not find record in above cache.
                                        if (dataRow != null && dataRow.Count() == 0)
                                        {
                                            dt = GetTableDataManager.GetTableData(pTableName, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
                                            CacheHelper<object>.AddOrUpdate(cacheName, dbContext.TenantID, dt);
                                            dataRow = dt.Select("ID=" + longValue);
                                        }
                                    }
                                    else if (Guid.TryParse(x, out GuidValue))
                                    {
                                        dataRow = dt.Select($"ID='{GuidValue}'");
                                        //Added below condition, when new record is added in Transaction table like Opportunity, but could not find record in above cache.
                                        if (dataRow != null && dataRow.Count() == 0)
                                        {
                                            dt = GetTableDataManager.GetTableData(pTableName, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
                                            CacheHelper<object>.AddOrUpdate(cacheName, dbContext.TenantID, dt);
                                            dataRow = dt.Select($"ID='{GuidValue}'");
                                        }
                                    }
                                }
                                if (dataRow != null && dataRow.Count() > 0)
                                {
                                    if (!string.IsNullOrEmpty(pTableName) && pTableName.Equals("assets", StringComparison.OrdinalIgnoreCase))
                                        pFieldName = DatabaseObjects.Columns.AssetTagNum;

                                    if (fieldName == DatabaseObjects.Columns.RequestTypeLookup)
                                    {
                                        if (!string.IsNullOrWhiteSpace(Convert.ToString(dataRow[0][pFieldName])))
                                        {
                                            string RequestType = (dataRow[0]["Category"] != null && dataRow[0]["Category"].ToString() != "" ? dataRow[0]["Category"] + " > " : "") + (dataRow[0]["SubCategory"] != null && dataRow[0]["SubCategory"].ToString() != "" ? dataRow[0]["SubCategory"] + " > " : "") + (Convert.ToString(dataRow[0][pFieldName]));
                                            listLookupValues.Add(RequestType);
                                        }
                                    }
                                    else if (!string.IsNullOrWhiteSpace(Convert.ToString(dataRow[0][pFieldName])))
                                        listLookupValues.Add(Convert.ToString(dataRow[0][pFieldName]));
                                    
                                }
                            });
                            outputValue = string.Join(Constants.Separator6, listLookupValues.ToArray());
                        }
                        else
                        {
                            ResolveLookupValues(field.ParentTableName, ref dt);                            
                            
                            List<string> listLookupValues = new List<string>();
                            lookupIds.ForEach(x =>
                            {
                                DataRow[] dataRow = null;
                                if (dt.Columns.Contains(DatabaseObjects.Columns.TicketId))
                                    dataRow = dt.Select($"{DatabaseObjects.Columns.TicketId}='{x}'");
                                if (dataRow != null && dataRow.Count() > 0)
                                {
                                    listLookupValues.Add(Convert.ToString(dataRow[0][pFieldName]));
                                }
                            });

                            if (listLookupValues != null && listLookupValues.Count > 0)
                                outputValue = string.Join(Constants.Separator6, listLookupValues.ToArray());
                            else
                                outputValue = values;
                        }
                    }

                    break;
                case FieldType.Percentage:
                    if (!string.IsNullOrEmpty(values))
                    {
                        outputValue = values + " %";
                    }

                    break;
                case FieldType.Currency:
                    if (!string.IsNullOrEmpty(values))
                    {
                        CultureInfo ci = new CultureInfo("en-US");
                        var symbol = ci.NumberFormat.CurrencySymbol;
                        outputValue = symbol + values;
                    }

                    break;
                case FieldType.Attachments:
                    if (!string.IsNullOrEmpty(values))
                    {
                        var data = GetTableDataManager.GetTableData(pTableName, $"FileID='{values}'").AsEnumerable().FirstOrDefault();
                        if (data != null)
                        {
                            outputValue = data.Field<string>("Name");
                        }
                    }
                    break;
                default:
                    outputValue = values;
                    break;
            }
            return outputValue;
        }

        private void ResolveLookupValues(string parentTableName, ref DataTable dt)
        {
            string moduleName = _moduleViewManager.GetModuleByTableName(parentTableName);
            string cacheName = string.Empty;
            if (string.IsNullOrEmpty(moduleName))
                cacheName = $"Lookup_{parentTableName}_{dbContext.TenantID}";
            else
                cacheName = $"AllTicket_{moduleName}";

            if (CacheHelper<object>.IsExists(cacheName, dbContext.TenantID))
            {
                dt = CacheHelper<object>.Get(cacheName, dbContext.TenantID) as DataTable;
                if (dt == null)
                {
                    dt = GetTableDataManager.GetTableData(parentTableName, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
                    CacheHelper<object>.AddOrUpdate(cacheName, dbContext.TenantID, dt);
                }
            }
            else
            {
                dt = GetTableDataManager.GetTableData(parentTableName, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
                CacheHelper<object>.AddOrUpdate(cacheName, dbContext.TenantID, dt);
            }            
        }

        public string GetFieldConfigurationIdByName(string fieldName, string values, string module = " ")
        {
            FieldConfiguration field = GetFieldByFieldName(fieldName);
            if (field == null)
                return values;
            DataTable dt;
            string outputValue = string.Empty;
            FieldType fType = FieldType.None;
            Enum.TryParse(field.Datatype, out fType);
            switch (fType)
            {
                case FieldType.UserField:
                    string commanames = dbContext.UserManager.CommaSeparatedIdsFrom(values, Constants.Separator6);
                    outputValue = commanames;
                    break;
                case FieldType.Lookup:
                    if (!string.IsNullOrEmpty(values))
                    {
                        List<System.Int64> listLookupValues = new List<System.Int64>();
                        string cacheName = "Lookup_" + field.ParentTableName + "_" + dbContext.TenantID;
                        if (CacheHelper<object>.IsExists(cacheName, dbContext.TenantID))
                            dt = CacheHelper<object>.Get(cacheName, dbContext.TenantID) as DataTable;
                        else
                        {
                            dt = GetTableDataManager.GetTableData(field.ParentTableName, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
                            CacheHelper<object>.AddOrUpdate(cacheName, dbContext.TenantID, dt);
                        }

                        if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ModuleNameLookup, field.ParentTableName))
                            listLookupValues = dt.Select($"{field.ParentFieldName} like ('%{uHelper.EscapeApostrophe(values)}%') and {DatabaseObjects.Columns.ModuleNameLookup} = '{module}'").AsEnumerable().Select(x => x.Field<System.Int64>(DatabaseObjects.Columns.ID)).ToList();
                        else
                            listLookupValues = dt.Select($"{field.ParentFieldName} like ('%{uHelper.EscapeApostrophe(values)}%') and {DatabaseObjects.Columns.Deleted} = '{false}'").AsEnumerable().Select(x => x.Field<System.Int64>(DatabaseObjects.Columns.ID)).ToList();

                        if (fieldName == DatabaseObjects.Columns.RequestTypeLookup)
                            listLookupValues = dt.Select($"{DatabaseObjects.Columns.ModuleNameLookup} = '{module}' and ({field.ParentFieldName} like ('%{uHelper.EscapeApostrophe(values)}%') or {DatabaseObjects.Columns.Title} like ('%{FormatRequestType(values)}%')) ").AsEnumerable().Select(x => x.Field<long>(DatabaseObjects.Columns.ID)).ToList();

                        //listLookupValues = dt.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.StageType).ToLower().Contains(values.ToLower())).Select(x => x.Field<System.Int64>(DatabaseObjects.Columns.ID)).ToList();
                        outputValue = string.Join(Constants.Separator6, listLookupValues.ToArray());
                    }
                    break;
                case FieldType.Choices:
                    if (!string.IsNullOrEmpty(values))
                    {
                        outputValue = $"'%{values}%'";
                    }
                    break;
                default:
                    outputValue = $"'%{values}%'"; //values;
                    break;
            }
            return outputValue;
        }

        private static string FormatRequestType(string value)
        {
            string val = uHelper.EscapeApostrophe(value);
            if (!string.IsNullOrWhiteSpace(val) && val.IndexOf(">") >= 0)
            {
                List<string> arr = val.Split('>').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
                return string.Join(" > ", arr);
            }
            return val;
        }

        public DataTable GetFieldConfiguration_Lookup_UserField()
        {
            var where_clause = "TenantID='" + this.dbContext.TenantID + "'" + " and (Datatype ='Lookup' or Datatype ='UserField')";
            //return this.GetDataTable(where_clause);
            return GetTableDataManager.GetTableData(DatabaseObjects.Tables.FieldConfiguration, where_clause);
        }

        public void RefreshCache()
        {
            List<FieldConfiguration> lstFieldConfiguration = this.Load();
            if (lstFieldConfiguration == null || lstFieldConfiguration.Count == 0)
                return;
            //Individual filed level cache, later it will be taken out in master branch
            foreach (FieldConfiguration field in lstFieldConfiguration)
            {
                CacheHelper<FieldConfiguration>.AddOrUpdate(field.FieldName, this.dbContext.TenantID, field);
                if (!string.IsNullOrEmpty(field.TableName))
                {
                    CacheHelper<FieldConfiguration>.AddOrUpdate(string.Format("{0}_{1}", field.FieldName, field.TableName), this.dbContext.TenantID, field);
                }
            }
            // To put all data from field configuration into cache 
            CacheHelper<object>.AddOrUpdate(string.Format("Available_FieldConfiguration_{0}", this.dbContext.TenantID), lstFieldConfiguration);
        }

        public FieldConfiguration Save(FieldConfiguration item)
        {
            if (item.ID > 0)
                this.Update(item);
            else
                this.Insert(item);
            return item;
        }
        public FieldConfiguration GetFieldByFieldName(string fieldName)
        {
            FieldConfiguration field = new FieldConfiguration();
            var dtResult = (List<FieldConfiguration>)CacheHelper<object>.Get(string.Format("Available_FieldConfiguration_{0}", this.dbContext.TenantID));
            if (dtResult == null || dtResult.Count==0)
            {
                CacheHelper<object>.AddOrUpdate(string.Format("Available_FieldConfiguration_{0}", this.dbContext.TenantID), this.Load());
                dtResult = (List<FieldConfiguration>)CacheHelper<object>.Get(string.Format("Available_FieldConfiguration_{0}", this.dbContext.TenantID));
            }

            if (dtResult != null && dtResult.Count > 0)
                field = (FieldConfiguration)dtResult.Where(x => x.FieldName == fieldName).FirstOrDefault();

            return field;
        }

        
    }
}
