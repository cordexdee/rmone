using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using uGovernIT.DAL;

namespace uGovernIT.DefaultConfig
{
    public class Helper
    {
        public static OleDbConnection GetOleDbConnection(string excelPath)
        {
            if (!System.IO.File.Exists(excelPath))
            {
                Log.WriteLog("ERROR: File not found - {0}", excelPath);
                return null;
            }

            // Open Excel file
            Log.WriteLog("-- Importing Excel file {0}", excelPath);

            OleDbConnection xlConn = new OleDbConnection(CreateXlConnectionString(excelPath));
            if (xlConn == null)
                return null;
            return xlConn;
        }

        // Excel Helpers
        public static String CreateXlConnectionString(string filePath)
        {
            string ext = null;
            if (filePath.EndsWith("xls", StringComparison.InvariantCultureIgnoreCase))
                ext = "xls";
            else if (filePath.EndsWith("xlsx", StringComparison.InvariantCultureIgnoreCase))
                ext = "xlsx";
            else
            {
                Log.WriteLog("Unsupported file format, need XLS or XLSX file", "CreateXlConnectionString");
                return null;
            }

            // NOTE: TO PREVENT DATA TRUNCATION OF TEXT FIELDS, Change TypeGuessRows key to 0 in:
            // HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Office\14.0\Access Connectivity Engine\Engines\Excel
            String connString = string.Empty;
            if (ext == "xls")
                connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + filePath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
            else if (ext == "xlsx")
                connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + filePath + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
            return connString;

        }

        public static string[] GetExcelSheetNames(OleDbConnection xlConn, string IndexSheet)
        {
            // If we find a special sheet called ExcelSheetLoadOrder then use that to get list
            // else just return alphabetical list
            System.Data.DataSet sheetNames = getXldata(xlConn, IndexSheet);
            DataTable dt = null;
            string[] excelSheets = null;
            if (sheetNames != null)
            {
                dt = sheetNames.Tables[0];
                excelSheets = new string[dt.Rows.Count];
                int i = 0;
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    excelSheets[i++] = row[0].ToString() + "$";
                }
            }
            else
            {
                Log.WriteLog("Index sheet {0} not found, loading in alphabetical order", IndexSheet);
                try
                {
                    xlConn.Open();
                }
                catch (Exception)
                {
                    Log.WriteLog("Could not open Excel file", "");
                    return null;
                }

                dt = xlConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                excelSheets = new string[dt.Rows.Count];
                int i = 0;
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    excelSheets[i++] = row["TABLE_NAME"].ToString();
                }

                xlConn.Close();
            }

            return excelSheets;
        }

        public static System.Data.DataSet getXldata(OleDbConnection xlConn, string sheetName)
        {
            string cmd = string.Format("select * from [{0}]", sheetName);

            System.Data.OleDb.OleDbCommand Comm = new System.Data.OleDb.OleDbCommand(cmd, xlConn);
            Comm.Connection = xlConn;

            System.Data.DataSet ds = new System.Data.DataSet();
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter();
            try
            {
                xlConn.Open();

                Comm.CommandType = System.Data.CommandType.Text;
                adapter.SelectCommand = Comm;
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                Log.WriteLog("No sheet found with name: {0}", sheetName);
                Log.WriteLog(ex.StackTrace);
                return null;
            }
            finally
            {
                xlConn.Close();
            }
            return ds;
        }


        /// <summary>
        /// It get column value from datarow 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public static object GetSPItemValue(DataRow item, string columnName)
        {
            try
            {
                if (item != null && item.Table.Columns.Contains(columnName) && item[columnName] != null)
                {
                    return item[columnName];
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "GetSPItemValue " + columnName);
                return string.Empty;
            }
        }

        /// <summary>
        ///It checks whether column exist in the datarow or not. 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public static bool IsSPItemExist(DataRow item, string columnValue)
        {
            try
            {
                if (item != null && item.Table.Columns.Contains(columnValue) && null != item[columnValue])
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "IsSPItemExist " + columnValue);
                return false;
            }
        }

        public static bool StringToBoolean(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            else
            {
                string strValue = value.Trim().ToLower();
                return (strValue == "1" || strValue == "true" || strValue == "yes" || strValue == "on");
            }
        }
        public static string getTableIDByModuleTitle(string TableName,string where)
        {           
                DataTable dt = uGITDAL.GetTable(TableName, where);
            if (dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["ID"]);
            else
                return "0"; 
        }
       
        public static string ConcatURLs(string url1, string url2)
        {
            char[] slashArrary = new char[] { '/' };

            // Remove trailing slash from first url
            string trimmedUrl1 = string.IsNullOrEmpty(url1) ? string.Empty : url1.Trim().TrimEnd(slashArrary);

            // Remove leading slash from secong url
            string trimmedUrl2 = string.IsNullOrEmpty(url2) ? string.Empty : url2.Trim().TrimStart(slashArrary);

            // Now concatenate the two using slash if needed
            // If url2 starts with query characters like ? & or # don't add slash in between
            string concatUrl = string.Empty;
            if (trimmedUrl2.Length > 0 && (Char.IsLetterOrDigit(trimmedUrl2, 0) || trimmedUrl2.Substring(0, 1) == "_"))
                concatUrl = string.Format("{0}/{1}", trimmedUrl1, trimmedUrl2);
            else
                concatUrl = string.Format("{0}{1}", trimmedUrl1, trimmedUrl2);

            return concatUrl;
        }

        public static Guid StringToGuid(object s)
        {
            try
            {
                Guid guid = new Guid(Convert.ToString(s));
                return guid;
            }
            catch (FormatException)
            {
                return Guid.Empty;
            }
        }
    }
}
