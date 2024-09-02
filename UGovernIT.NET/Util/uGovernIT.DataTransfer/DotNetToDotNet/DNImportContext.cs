using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DataTransfer.Infratructure;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.DataTransfer.DotNetToDotNet
{
    public class DNImportContext : ImportContext
    {
        public ApplicationContext SourceAppContext { get; set; }

        public string GetTargetUserValue(string sourceValue)
        {
            return Convert.ToString(GetTargetValue(sourceValue, null, null, "User"));
        }

        public long? GetTargetLookupValueOptionLong(object sourceValue, string fieldName, string table)
        {
            object value = GetTargetValue(sourceValue, fieldName, table, "Lookup");
            if (string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(value)))
                return null;
            else
                return UGITUtility.StringToLong(value);
        }

        public long GetTargetLookupValueLong(object sourceValue, string fieldName, string table)
        {
            object value = GetTargetValue(sourceValue, fieldName, table, "Lookup");
            return UGITUtility.StringToLong(value);
        }

        public string GetTargetLookupValue(object sourceValue, string fieldName, string table)
        {
            return Convert.ToString(GetTargetValue(sourceValue, fieldName, table, "Lookup"));
        }

        public object GetTargetValue(object sourceValue, string fieldName, string table, string type)
        {
            object targetValues = string.Empty;
            FieldConfigurationManager fieldMgr = new FieldConfigurationManager(SourceAppContext);

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
