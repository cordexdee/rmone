using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

/// <summary>
/// Summary description for QueryBuilder
/// </summary>
/// 

namespace Utils
{
    public class QueryBuilder
    {
        public QueryBuilder()
        {

        }

        public static string BuildInsertQuery(string table, Dictionary<string, object> column_values)
        {
            string sql = "insert into " + table + " ";

            string columns_string = "(";
            string values_string = "(";
            foreach (string key in column_values.Keys)
            {
                columns_string += key + ",";
                object value = column_values[key];
                if (value == null)
                {
                    values_string += "null,";
                }
                else if (typeof(String) == value.GetType())
                {
                    values_string += "'" + value.ToString() + "',";
                }
                else if (typeof(Boolean) == value.GetType())
                {
                    values_string += (((Boolean)value) ? 1 : 0) + ",";
                }
                else
                    values_string += value + ",";
            }
            if (columns_string.EndsWith(","))
            {
                columns_string = columns_string.Remove(columns_string.LastIndexOf(","));
                columns_string += ")";
            }
            if (values_string.EndsWith(","))
            {
                values_string = values_string.Remove(values_string.LastIndexOf(","));
                values_string += ")";
            }

            sql += columns_string + " Values " + values_string;
            return sql + "; select @@Identity as 'Id'";
        }
        public static SqlCommand BuildDeleteQuery(string table, string fieldname, string fieldvalue)
        {
            SqlCommand cmd = new SqlCommand();
            if (!string.IsNullOrEmpty(fieldname) && !string.IsNullOrEmpty(fieldvalue))
            {
                cmd.CommandText = "Delete from " + table + " where " + fieldname + "=@fieldValue";
                cmd.Parameters.AddWithValue("@fieldValue", fieldvalue);
            }
            else
            {
                cmd.CommandText = "Delete from " + table;
            }
            
            return cmd;
        }

        public static string BuildUpdateQuery(string table, long id, Dictionary<string, object> to_be_updated)
        {
            String sql = "update " + table + " set ";
            foreach (string key in to_be_updated.Keys)
            {
                sql += (key + "=");
                object value = to_be_updated[key];
                if (value == null)
                {
                    sql += "null";
                }
                else if (typeof(String) == value.GetType())
                {
                    sql += "'" + value.ToString() + "'";
                }
                else if (typeof(Boolean) == value.GetType())
                {
                    sql += ((Boolean)value) ? 1 : 0;
                }
                else
                    sql += value;

                sql += ",";
            }
            sql = StringUtils.removeTrailingSeparator(sql, ",");

            sql += " where Id=" + id;

            return sql;
        }
        public static SqlCommand BuildUpdateWithParams(string table, Dictionary<string, object> values, string whereClause, string whereParam, long id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE ")
               .Append(table)
               .Append(" SET ");

            string setString = "";

            foreach (KeyValuePair<string, object> kVp in values)
            {
                setString += kVp.Key + "=@" + kVp.Key + ",";
            }

            setString = StringUtils.removeTrailingSeparator(setString, ",");

            sql.Append(setString);

            if (!string.IsNullOrEmpty(whereClause))
                sql.Append(" WHERE ").Append(whereClause + whereParam);

            SqlCommand cmd = new SqlCommand(sql.ToString());

            //Add the sql parameters to the command
            foreach (KeyValuePair<string, object> kVp in values)
            {
                cmd.Parameters.AddWithValue("@" + kVp.Key, kVp.Value);
            }

            if (!string.IsNullOrEmpty(whereClause))
            {
                cmd.Parameters.AddWithValue(whereParam, id);
            }
            return cmd;
        }
        public static SqlCommand BuildUpdateWithParamsString(string table, Dictionary<string, object> values, string whereClause, string whereParam, string id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE ")
               .Append(table)
               .Append(" SET ");

            string setString = "";

            foreach (KeyValuePair<string, object> kVp in values)
            {
                setString += kVp.Key + "=@" + kVp.Key + ",";
            }

            setString = StringUtils.removeTrailingSeparator(setString, ",");

            sql.Append(setString);

            if (!string.IsNullOrEmpty(whereClause))
                sql.Append(" WHERE ").Append(whereClause + whereParam);

            SqlCommand cmd = new SqlCommand(sql.ToString());

            //Add the sql parameters to the command
            foreach (KeyValuePair<string, object> kVp in values)
            {
                cmd.Parameters.AddWithValue("@" + kVp.Key, kVp.Value);
            }

            if (!string.IsNullOrEmpty(whereClause))
            {
                cmd.Parameters.AddWithValue(whereParam, id);
            }
            return cmd;
        }
        

        public static string BuildInsertSelect(string table, string[] COLUMNS)
        {
            string sql = "insert into " + table + " (";
            foreach (string column in COLUMNS)
            {
                sql += column + ",";
            }
            sql = StringUtils.removeTrailingSeparator(sql, ",");
            sql += ") values select ";
            foreach (string column in COLUMNS)
            {
                sql += column + ",";
            }
            sql = StringUtils.removeTrailingSeparator(sql, ",");
            sql += " from " + table;

            return sql;
        }

        public static SqlCommand BuildInsertWithParams(string table, Dictionary<string, object> values)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO ")
               .Append(table);

            string columnString = "(";
            string valuesString = "(";
            foreach (KeyValuePair<string, object> kVp in values)
            {
                columnString += kVp.Key + ",";
                valuesString += "@" + kVp.Key + ",";
            }

            columnString = StringUtils.removeTrailingSeparator(columnString, ",");
            valuesString = StringUtils.removeTrailingSeparator(valuesString, ",");

            columnString += ")";
            valuesString += ")";

            //Append the column and values string
            sql.Append(columnString)
               .Append(" VALUES ")
               .Append(valuesString);

            sql.Append(";")
               .Append("SELECT CAST(scope_identity() AS int)");

            SqlCommand cmd = new SqlCommand(sql.ToString());

            //Add the sql parameters to the command
            foreach (KeyValuePair<string, object> kVp in values)
            {
                cmd.Parameters.AddWithValue("@" + kVp.Key, kVp.Value);
            }
            return cmd;
        }

       public static string BuildSelectQuery(string table, string[] columns)
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            if (columns == null)
                sql.Append(" * ");
            else
            {
                foreach (string column in columns)
                {
                    sql.Append(column).Append(",");
                }

                if (sql.ToString().EndsWith(","))
                {
                    sql.Remove(sql.Length - 1, 1);
                }
            }

            sql.Append(" FROM ")
               .Append(table);
            return sql.ToString();
        }

        public static string BuildSelectQuery(string table, string[] columns, int top)
        {
            StringBuilder sql = new StringBuilder("SELECT ");

            sql.Append(top >= 0 ? " TOP " + top : " ");

            if (columns == null)
                sql.Append(" * ");
            else
            {
                foreach (string column in columns)
                {
                    sql.Append(column).Append(",");
                }

                if (sql.ToString().EndsWith(","))
                {
                    sql.Remove(sql.Length - 1, 1);
                }
            }

            sql.Append(" FROM ")
               .Append(table);
            return sql.ToString();
        }
    }
}
