using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using uGovernIT.Util.Log;

/// <summary>
/// Summary description for SqlReaderUtils
/// </summary>
public class SqlReaderUtils
{
    public SqlReaderUtils()
    {
        //
        // TODO: Add constructor logic here
        // 
    }

    public static int getInt(DbDataReader reader, int column)
    {
        if (reader.IsDBNull(column))
            return 0;
        else
        {
            try
            {
                return Int32.Parse(reader.GetValue(column).ToString());
            }
            catch
            {
                return 0;
            }
        }
    }

    public static long getLong(DbDataReader reader, int column)
    {
        if (reader.IsDBNull(column))
            return 0;
        else
        {
            try
            {
                return Int64.Parse(reader.GetValue(column).ToString());
            }
            catch
            {
                return 0;
            }
        }
    }

    public static string getString(DbDataReader reader, int column)
    {
        if (reader.IsDBNull(column))
            return null;
        else
            return reader.GetValue(column).ToString();
    }

    public static float getFloat(DbDataReader reader, int column)
    {
        if (reader.IsDBNull(column))
            return 0;
        else
        {
            try
            {
                return Single.Parse(reader.GetValue(column).ToString());
            }
            catch
            {
                return 0;
            }
        }
    }

    public static bool getBoolean(DbDataReader reader, int column)
    {
        if (reader.IsDBNull(column))
            return false;
        else
        {
            try
            {
                return Boolean.Parse(reader.GetValue(column).ToString());
            }
            catch
            {
                return false;
            }
        }
    }

    internal static object getObject(string sql)
    {
        using (SqlConnection connection = DBConnection.GetSqlConnection())
        {
            try
            {
                object value = DBConnection.ExecuteStatementWithReturn(connection, sql);
                connection.Close();
                return value;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
        }
    }

    internal static long getLong(string sql)
    {
        Object value = getObject(sql);
        try
        {
            return Int64.Parse(value.ToString());
        }
        catch (Exception ex)
        {
            ULog.WriteException(ex);
            return 0;
        }
    }

    public static string getString(string sql)
    {
        Object value = getObject(sql);
        if (value == null)
            return null;
        try
        {
            return value.ToString();
        }
        catch (Exception ex)
        {
            ULog.WriteException(ex);
            return null;
        }
    }

    public static int getInt(string sql)
    {
        Object value = getObject(sql);
        try
        {
            return Int32.Parse(value.ToString());
        }
        catch (Exception ex)
        {
            ULog.WriteException(ex);
            return 0;
        }
    }

    internal static float getFloat(string sql)
    {
        Object value = getObject(sql);
        try
        {
            return Single.Parse(value.ToString());
        }
        catch (Exception ex)
        {
            ULog.WriteException(ex);
            return 0;
        }
    }

    internal static DateTime getDate(DbDataReader reader, int column)
    {
        if (reader.IsDBNull(column))
        {
            return DateTime.MinValue;
        }
        else
            return reader.GetDateTime(column);
    }

    /**
     * Returns a single column select statement, and makes a list of them.
     * */
    public static List<object> getList(string sql)
    {
        List<object> list = new List<object>();
        SqlConnection connection = DBConnection.GetSqlConnection();
        SqlDataReader reader = DBConnection.ExecuteReader(connection, sql);
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                list.Add(reader.GetValue(0));
            }
        }
        reader.Close();
        connection.Close();
        return list;
    }

    internal static bool getBoolean(string sql)
    {
        object value = getObject(sql);
        try
        {
            return Boolean.Parse(value.ToString());
        }
        catch (Exception ex)
        {
            ULog.WriteException(ex);
            return false;
        }
    }
}

