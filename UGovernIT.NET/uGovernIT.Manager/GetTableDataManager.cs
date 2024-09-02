using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using uGovernIT.DAL;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class GetTableDataManager
    {
        public static string GetCacheKey(string moduleName, string tenantId)
        {
            return $"{moduleName}_{tenantId}";
        }

        public static DataTable GetTableDataUsingQuery(string sqlQuery)
        {
            return uGITDAL.GetTableDataUsingQuery(sqlQuery);
        }

        public static DataTable GetTenantDataUsingQueries(string sqlQuery)
        {
            return uGITDAL.GetTenantDataUsingQueries(sqlQuery);
        }

        public static DataTable GetTableData(string tableName, string where)
        {           
            return uGITDAL.GetTable(tableName, where);
        }

        public static DataTable ExecuteQuery(string Query)
        {
            return uGITDAL.ExecuteQuery(Query);
        }

        public static bool ExecuteNonQuery(string Query)
        {
            return uGITDAL.ExecuteNonQuery(Query);
        }

        public static DataTable ExecuteQueryTenant(string Query)
        {
            return uGITDAL.ExecuteQueryTenant(Query);
        }

        public static bool IsExist(string query, bool tenantDb = false)
        {
            return uGITDAL.IsExist(query, tenantDb);
        }

        public static string GetSingleValueByIdFromCache(string tableName, string Id, string tenantId)
        {
            var returnValue = "";
            var cacheKey = GetCacheKey(tableName, tenantId);

            var cacheDataTable = (DataTable)CacheHelper<object>.Get(cacheKey, tenantId);
            if (cacheDataTable == null)
            {
                cacheDataTable = uGITDAL.GetTable(tableName, $"TenantID='{tenantId}'");
                CacheHelper<object>.AddOrUpdate(cacheKey, tenantId, cacheDataTable);
            }
            var dataRow = cacheDataTable.AsEnumerable()
                                        .FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.ID) == Convert.ToInt64(Id));
            if (dataRow != null)
            {
                returnValue = Convert.ToString(dataRow[DatabaseObjects.Columns.Title]);
            }

            return returnValue;
        }

        public static DataTable GetTableData(string TableName)
        {
            return uGITDAL.GetTable(TableName);
        }

        public static DataTable GetTableData(string TableName, dynamic where, string dummyParam = null)
        {
            return uGITDAL.GetTable(TableName, Convert.ToString(where));
        }

        /// <summary>
        /// Method to return selected columns from table. 
        /// </summary>
        public static DataTable GetTableData(string TableName, dynamic where, dynamic columns, string dummyParam = null)
        {
            return uGITDAL.GetTable(TableName, Convert.ToString(where), Convert.ToString(columns));
        }

        /// <summary>
        /// Method to return single value of a column from table, by ID. 
        /// </summary>
        public static object GetSingleValueByID(string tableName, string columnName, string ID, string TenantId)
        {
            return uGITDAL.GetSingleValueByID(tableName, columnName, ID, TenantId);
        }

        /// <summary>
        /// Method to return single value of a column from table, by TicketId. 
        /// </summary>
        public static object GetSingleValueByTicketId(string tableName, string columnName, string TicketId, string TenantId)
        {
            return uGITDAL.GetSingleValueByTicketId(tableName, columnName, TicketId, TenantId);
        }

        /// <summary>
        /// Method to return table structure (Table without records). 
        /// </summary>
        public static DataTable GetTableStructure(string tableName)
        {
            return uGITDAL.GetTableStructure(tableName);
        }

        public static int? AddItem<T>(string table, Dictionary<string, object> values)
        {
            return uGITDAL.insertData(table, values);
        }

        public static int? UpdateItem<T>(string table, long id, Dictionary<string, object> values)
        {
            return uGITDAL.UpdateData(table, id, values);
        }

        public static int? delete<T>(string table, string columnname, string value)
        {
            return uGITDAL.DeleteData(table, columnname, value);
        }

        public static DataRow[] GetDataRow(string listName, string columnName, object columnValue)
        {
            DataTable table = GetTableData(listName);
            if (table != null && table.Columns.Contains(columnName))
            {
                string expression = string.Empty;
                if (columnValue != null)
                {
                    expression = string.Format("{0}={1}", columnName, columnValue);
                    if (columnValue is string)
                    {
                        expression = string.Format("{0}='{1}'", columnName, columnValue);
                    }
                }
                else
                {
                    expression = string.Format("{0} is null", columnName);
                }
                DataRow[] rows = table.Select(expression);
                return rows;
            }
            return new DataRow[0];
        }

        public static bool IsLookaheadTicketExists(string moduleTable, string tenantId, string title, string requestTypeLookup, bool? rpaFlag = null)
        {
            return uGITDAL.IsLookaheadTicketExists(moduleTable, tenantId, title, requestTypeLookup, rpaFlag);
        }

        public static bool IsResetPasswordExists(string tenantId, string title)
        {
            return uGITDAL.IsResetPasswordExists(tenantId, title);
        }

        public static DataTable autoSetRequestor(string moduleTable, string tenantId, string title)
        {
            return uGITDAL.autoSetRequestor(moduleTable, tenantId, title);
        }

        public static DataTable autoSetRequestType(string moduleTable, string tenantId, string title)
        {
            return uGITDAL.autoSetRequestType(moduleTable, tenantId, title);
        }

        public static DataTable autoSetResolutionTime(string moduleTable, string tenantId, string title)
        {
            return uGITDAL.autoSetResolutionTime(moduleTable, tenantId, title);
        }

        public static DataTable autoSetPRP(string moduleTable, string tenantId, string title, string cloumn, int status, bool rpaFlag = false)
        {
            return uGITDAL.autoSetPRP(moduleTable, tenantId, title, cloumn, status, rpaFlag);
        }

        public static DataTable autoSetPRP(string moduleTable, string tenantId, string title, string cloumn, string status)
        {
            return uGITDAL.autoSetPRP(moduleTable, tenantId, title, cloumn, status);
        }

        public static bool ISAsset(string tenantId, string title, string tableName)
        {
            return uGITDAL.IsAsset(tenantId, title, tableName);
        }

        public static bool bulkupload(DataTable tableName, string destinationtable)
        {
            // ModuleFormLayoutStore mStore = new ModuleFormLayoutStore();           
            return uGITDAL.bulkupload(tableName, destinationtable);
        }
        public static DataTable GetData(string storeproc, Dictionary<string, object> values)
        {
            return uGITDAL.ExecuteDataSetWithParameters("usp_Get"+storeproc, values);
        }
        public static DataSet GetDataSet(string storeproc, Dictionary<string, object> values)
        {
            return uGITDAL.ExecuteDataSet_WithParameters("usp_Get" + storeproc, values);
        }
        public static string CreateTempTable()
        {
            return uGITDAL.CreateTempTable();
        }
        public static bool DeleteData(string storeproc, Dictionary<string, object> values, ApplicationContext context)
        {
            try
            {
                uGITDAL.ExecuteDataSetWithParameters(storeproc, values);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
