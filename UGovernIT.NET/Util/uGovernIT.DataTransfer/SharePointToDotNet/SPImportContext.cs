using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Infratructure;
using uGovernIT.DataTransfer.Infratructure;
using uGovernIT.DataTransfer.SharePointToDotNet;
using uGovernIT.Manager;
using uGovernIT.Utility;
using ClientOM = Microsoft.SharePoint.Client;

namespace uGovernIT.DataTransfer.SharePointToDotNet
{
    public class SPImportContext : ImportContext
    {
        public SiteAuthentication SPSite { get; set; }
        private Hashtable SPListMap;
        public ApplicationContext SourceAppContext { get; set; }

        public SPImportContext():base()
        {
            SPListMap = new Hashtable();
        }

        public string GetTargetUserValue(string sourceValue)
        {
            return Convert.ToString(GetTargetValue(sourceValue, null, null, "User"));
        }
        public long? GetTargetValueAsOptionalLong(ClientOM.ClientContext spContext, ClientOM.List list, ClientOM.ListItem item, string fieldName)
        {
            string targetID = GetTargetValue(spContext, list, item, fieldName);
            if (string.IsNullOrWhiteSpace(targetID))
                return null;

            return UGITUtility.StringToLong(targetID);
        }

        public long GetTargetValueAsLong(ClientOM.ClientContext spContext, ClientOM.List list, ClientOM.ListItem item, string fieldName)
        {
            string targetID = GetTargetValue(spContext, list, item, fieldName);
            return UGITUtility.StringToLong(targetID);
        }

        public string GetTargetValue(ClientOM.ClientContext spContext, ClientOM.List list, ClientOM.ListItem item, string targetFieldName, DataRow sourceRow = null, string sourceFieldName = null)
        {
            string targetValue = UGITUtility.ObjectToString(item[targetFieldName]);
              
            ClientOM.Field field = list.Fields.ToList().FirstOrDefault(x => x.InternalName == targetFieldName);
            if (!string.IsNullOrWhiteSpace(targetValue))
            {

                if (field is ClientOM.FieldLookup)
                {
                    ClientOM.FieldLookup lookupField = field as ClientOM.FieldLookup;
                    string lookupListName = GetSPListName(spContext, lookupField.LookupList);

                    MappedItemList mappedList = GetMappedList(lookupListName);

                    if (item[targetFieldName] is ClientOM.FieldLookupValue)
                    {
                        ClientOM.FieldLookupValue lookupValue = item[targetFieldName] as ClientOM.FieldLookupValue;
                        if (mappedList == null || lookupValue == null || lookupValue.LookupId == 0)
                            return null;

                        targetValue = mappedList.GetTargetID(lookupValue.LookupId.ToString());
                        if (string.IsNullOrWhiteSpace(targetValue))
                            targetValue = null;
                    }
                    else if (item[targetFieldName] is ClientOM.FieldLookupValue[])
                    {
                        ClientOM.FieldLookupValue[] lookupValue = item[targetFieldName] as ClientOM.FieldLookupValue[];
                        if (mappedList == null || lookupValue == null || lookupValue.Length == 0)
                            return null;

                        targetValue = mappedList.GetTargetIDs(lookupValue.Select(x => x.LookupId.ToString()).ToList());
                        if (string.IsNullOrWhiteSpace(targetValue))
                            targetValue = null;
                    }
                }
                else if (field is ClientOM.FieldUser)
                {
                    ClientOM.FieldUser userField = field as ClientOM.FieldUser;
                    MappedItemList mappedList = GetMappedList(SPDatabaseObjects.Lists.UserInformationList);

                    if (item[targetFieldName] is ClientOM.FieldUserValue)
                    {
                        ClientOM.FieldUserValue lookupValue = item[targetFieldName] as ClientOM.FieldUserValue;
                        if (mappedList == null || lookupValue == null || lookupValue.LookupId == 0)
                            return null;

                        targetValue = mappedList.GetTargetID(lookupValue.LookupId.ToString());
                        if (string.IsNullOrWhiteSpace(targetValue))
                            targetValue = null;
                    }
                    else if (item[targetFieldName] is ClientOM.FieldUserValue[])
                    {
                        ClientOM.FieldUserValue[] lookupValue = item[targetFieldName] as ClientOM.FieldUserValue[];
                        if (mappedList == null || lookupValue == null || lookupValue.Length == 0)
                            return null;

                        targetValue = mappedList.GetTargetIDs(lookupValue.Select(x => x.LookupId.ToString()).ToList());
                        if (string.IsNullOrWhiteSpace(targetValue))
                            targetValue = null;
                    }
                }
                else if (field is ClientOM.FieldDateTime)
                {
                    targetValue = Convert.ToString(UGITUtility.GetObjetToDateTime(targetValue));
                }
                else if (field.FieldTypeKind == ClientOM.FieldType.Boolean)
                {
                    targetValue = UGITUtility.StringToBoolean(targetValue).ToString();
                }
            }

            if (!string.IsNullOrWhiteSpace(targetValue))
            {
                if (targetFieldName == SPDatabaseObjects.Columns.TicketActualHours)
                {
                    targetValue = UGITUtility.StringToInt(targetValue).ToString();
                }
            }

            if (sourceRow != null && !string.IsNullOrWhiteSpace(sourceFieldName))
            {
                if (sourceRow.Table.Columns.Contains(sourceFieldName))
                {
                    DataColumn tableColumn = sourceRow.Table.Columns[sourceFieldName];

                    if (tableColumn.DataType == typeof(bool))
                    {
                        targetValue = UGITUtility.StringToBoolean(targetValue).ToString();
                    }
                    else if (tableColumn.DataType == typeof(int))
                    {
                        targetValue = UGITUtility.StringToInt(targetValue).ToString();
                    }
                    else if (tableColumn.DataType == typeof(long))
                    {
                        targetValue = UGITUtility.StringToLong(targetValue).ToString();
                    }
                    else if (tableColumn.DataType == typeof(DateTime))
                    {

                        DateTime datetime = DateTime.MinValue;
                        if (!string.IsNullOrWhiteSpace(targetValue))
                           datetime = UGITUtility.GetObjetToDateTime(targetValue);

                        if (tableColumn.AllowDBNull)
                        {
                            if (datetime == DateTime.MinValue)
                            {
                                targetValue = null;
                            }
                            else
                            {
                                targetValue = datetime.ToString();
                            }
                        }
                        else
                        {
                            if (datetime == DateTime.MinValue)
                            {
                                targetValue = SqlDateTime.MinValue.Value.ToString();
                            }
                            else
                            {
                                targetValue = datetime.ToString();
                            }
                        }
                    }
                }
            }

            return targetValue;
        }

        public string CovertActionUserTypes(string userTypes, Dictionary<string, string> columnsMapping)
        {
            if (string.IsNullOrWhiteSpace(userTypes))
                return userTypes;

            List<string> validFields = new List<string>();
            List<string> types = UGITUtility.ConvertStringToList(userTypes, Constants.Separator);
            foreach (string type in types)
            {
                if (columnsMapping.ContainsKey(type)) {
                    validFields.Add(columnsMapping[type]);
                }
                else {
                    validFields.Add(type);
                }
            }
            return string.Join(Constants.Separator, validFields);
        }

        public string GetTargetFieldName(ClientOM.List spList, ClientOM.Field spField, DataRow sourceItem)
        {
            string targetName = spField.InternalName;
            if (spField.InternalName == "TicketStageActionUsers")
            {
                targetName = "StageActionUsersUser";
            }
            if (spField.InternalName == "TotalActualHours")
            {
                targetName = "ActualHours";
            }
            if (spField.InternalName == "TicketRequestTypeCategory")
            {
                targetName = "RequestTypeCategory";
            }
            if (spField.InternalName == "TicketBusinessManager2")
            {
                targetName = "TicketBusinessManager2User";
            }
            if (spField.InternalName == "TicketRequestTypeSubCategory")
            {
                targetName = "RequestTypeSubCategory";
            }
            if (spField.InternalName == "TicketRequestTypeWorkflow")
            {
                targetName = "RequestTypeWorkflow";
            }
            if (spField.InternalName == "CategoryName")
            {
                targetName = "CategoryNameChoice";
            }
            if (spField.InternalName == "ServiceTitleLookup")
            {
                targetName = "ServiceLookup";
            }
            if (sourceItem.Table.Columns.Contains(targetName))
            {
                return targetName;
            }

            if (targetName.StartsWith("Ticket", StringComparison.OrdinalIgnoreCase))
            {
                targetName = string.Join("", spField.InternalName.Split(new string[] { "Ticket" }, StringSplitOptions.RemoveEmptyEntries).Skip(0));
                if (sourceItem.Table.Columns.Contains(targetName))
                {
                    return targetName;
                }
            }


            if (spField.FieldTypeKind == ClientOM.FieldType.Lookup && !targetName.EndsWith("lookup", StringComparison.OrdinalIgnoreCase))
            {
                if (sourceItem.Table.Columns.Contains($"{targetName}Lookup"))
                {
                    return $"{targetName}Lookup";
                }
            }
            else if (spField.FieldTypeKind == ClientOM.FieldType.User && !targetName.EndsWith("user", StringComparison.OrdinalIgnoreCase))
            {
                if (sourceItem.Table.Columns.Contains($"{targetName}User"))
                {
                    return $"{targetName}User";
                }
            }
            else if (spField.FieldTypeKind == ClientOM.FieldType.Choice && !targetName.EndsWith("choice", StringComparison.OrdinalIgnoreCase))
            {
                if (sourceItem.Table.Columns.Contains($"{targetName}Choice"))
                {
                    return $"{targetName}Choice";
                }
            }

            return null;
        }

        public string GetSPListName(ClientOM.ClientContext spContext, string listID)
        {
            string listName = string.Empty;
            if (SPListMap.ContainsKey(listID))
            {
                listName = Convert.ToString(SPListMap[listID]);
            }
            else
            {
                ClientOM.List lookupList = spContext.Web.Lists.GetById(Guid.Parse(listID));
                spContext.Load(lookupList);
                spContext.ExecuteQuery();
                listName = lookupList.Title;
                SPListMap.Add(listID, listName);
            }

            return listName;
        }
        public long? GetTargetLookupValueOptionLong(object sourceValue, string fieldName, string table)
        {
            object value = GetTargetValue(sourceValue, fieldName, table, "Lookup");
            if (string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(value)))
                return null;
            else
                return UGITUtility.StringToLong(value);
        }
        public object GetTargetValue(object sourceValue, string fieldName, string table, string type)
        {
            object targetValues = string.Empty;
            FieldConfigurationManager fieldMgr = new FieldConfigurationManager(AppContext);

            if (string.IsNullOrWhiteSpace(Convert.ToString(sourceValue)))
                return sourceValue;


            if (string.IsNullOrWhiteSpace(type) && !string.IsNullOrWhiteSpace(fieldName))
            {
                if (fieldName.EndsWith("User", StringComparison.OrdinalIgnoreCase))
                    type = "User";
                else if (fieldName.EndsWith("Lookup", StringComparison.OrdinalIgnoreCase))
                    type = "Lookup";
            }

            if (type == "Lookup")
            {
                FieldConfiguration field = fieldMgr.GetFieldByFieldName(fieldName, table);
                if (field != null)
                {
                    MappedItemList mappedList = this.GetMappedList(field.ParentTableName);
                    targetValues = mappedList.GetTargetIDs(UGITUtility.ConvertStringToList(Convert.ToString(sourceValue), Constants.Separator));
                    if (Convert.ToString(targetValues) == string.Empty)
                        targetValues = null;
                }
                else
                {
                    if (!string.IsNullOrEmpty(table))
                    {
                        MappedItemList mappedList = this.GetMappedList(table);
                        targetValues = mappedList.GetTargetIDs(UGITUtility.ConvertStringToList(Convert.ToString(sourceValue), Constants.Separator));
                        if (Convert.ToString(targetValues) == string.Empty)
                            targetValues = null;
                    }
                    else
                        targetValues = null;
                }
            }
            else if (type == "User")
            {
                MappedItemList userMapped = this.GetMappedList(DatabaseObjects.Tables.AspNetUsers);
                targetValues = userMapped.GetTargetIDs(UGITUtility.ConvertStringToList(Convert.ToString(sourceValue), Constants.Separator));
            }
            else if (type == "Attachment")
            {
                MappedItemList userMapped = this.GetMappedList(DatabaseObjects.Tables.Documents);
                targetValues = userMapped.GetTargetIDs(UGITUtility.ConvertStringToList(Convert.ToString(sourceValue), Constants.Separator6));
            }
            else
            {
                targetValues = sourceValue;
            }

            return targetValues;
        }

    }
}
