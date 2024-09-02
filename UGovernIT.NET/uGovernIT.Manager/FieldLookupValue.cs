using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
   public class FieldLookupValue
    {
        public string Value { get; set; }
        public int ID { get; set; }
        public string IDs { get; set; }
        public string Values { get; set; }
        public string tableName { get; set; }
        public FieldLookupValue()
        {

        }
        public FieldLookupValue(int id, string parentColName,string tableName)
        {
            DataTable dt = TicketDal.GetLookUpData(tableName, parentColName, id);
            if(dt.Rows.Count >0)
            {
                ID = Convert.ToInt32(dt.Rows[0][DatabaseObjects.Columns.Id]);
                Value = Convert.ToString(dt.Rows[0][DatabaseObjects.Columns.Title]);
            }
        }
        public FieldLookupValue(string ids, string parentColName, string tableName)
        {
            List<string> Ids = ids.Split(',').ToList();
            List<string> lstvalues = new List<string>();
            Ids.ForEach(x =>
            {
                DataTable dt = TicketDal.GetLookUpData(tableName, parentColName, Convert.ToInt32(x));
                if (dt.Rows.Count > 0)
                {
                    lstvalues.Add(Convert.ToString(dt.Rows[0][DatabaseObjects.Columns.Title]));
                }
            }
            );
            IDs = ids;
            Values = string.Join(",", lstvalues.ToArray());


        }



        public static string ConcatenateValues(List<LookupValue> lookups, string separator)
        {
            string values = string.Empty;
            if (separator == null)
                separator = Constants.Separator;
            if (lookups != null && lookups.Count > 0)
            {
                values = string.Join(separator, lookups.Select(x => x.Value).ToArray());
            }
            return values;
        }

        public static string ConcatenateIDs(List<LookupValue> lookups, string separator)
        {
            string ids = string.Empty;
            if (separator == null)
                separator = Constants.Separator;
            if (lookups != null && lookups.Count > 0)
            {
                ids = string.Join(separator, lookups.Select(x => x.IDValue.ToString()).ToArray());
            }
            return ids;
        }

        public static string ConcatenateBoth(List<LookupValue> lookups, string separator)
        {
            string values = string.Empty;
            if (separator == null)
                separator = Constants.Separator;
            if (lookups != null && lookups.Count > 0)
            {
                values = string.Join(separator, lookups.Where(x => x.IDValue > 0).Select(x => string.Format("{0}{1}{2}", x.IDValue, separator, x.Value)).ToArray());
            }
            return values;
        }

        public static string ConcatenateBoth(LookupValue lookup, string separator)
        {
            string values = string.Empty;
            if (separator == null)
                separator = Constants.Separator;
            if (lookup != null && lookup.IDValue > 0)
            {
                values = string.Format("{0}{1}{2}", lookup.IDValue, separator, lookup.Value);
            }
            return values;
        }


        public static string GetValue(LookupValue user)
        {
            if (user != null)
                return user.Value;
            return string.Empty;
        }

        public static long GetID(LookupValue user)
        {
            if (user != null)
                return user.IDValue;

            return 0;
        }
        public static string GetValuesByDatabase(string TableName, string ReturnFieldName, string ConditionFieldName, string ConditionValue)
        {
            SqlDataReader dr = null;
            string sql = "select " + ReturnFieldName + " from " + TableName + " where " + ConditionFieldName + "=@fieldValue";
            using (SqlConnection con = DBConnection.GetSqlConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@fieldValue", ConditionValue);
                dr = DBConnection.ExecuteReader(con, cmd);
            }
            return dr[ReturnFieldName].ToString();
        }
    }

}
