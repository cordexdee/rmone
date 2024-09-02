using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using uGovernIT.DAL.Infratructure;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using Utils;

namespace uGovernIT.DAL
{
    public static class uGITDAL
    {
        static string connectionStringnew = Convert.ToString(ConfigurationManager.ConnectionStrings["cnn"]);
        static string tenantcnn = Convert.ToString(ConfigurationManager.ConnectionStrings["tenantcnn"]);

        public static int? insertData(string table, Dictionary<string, object> values)
        {
            SqlCommand query = QueryBuilder.BuildInsertWithParams(table, values);
            using (SqlConnection connection = new SqlConnection(connectionStringnew))
            {
                SqlCommand cmd = query;
                cmd.Connection = connection;
                cmd.Connection.Open();
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
               
            }
        }

        public static int? UpdateData(string table, long id, Dictionary<string, object> values)
        {
            SqlCommand query = QueryBuilder.BuildUpdateWithParams(table, values, "ID=", "@ID", id);
            using (SqlConnection connection = new SqlConnection(connectionStringnew))
            {
                SqlCommand cmd = query;
                cmd.Connection = connection;
                cmd.Connection.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static DataTable GetTableDataUsingQuery(string sqlQuery)
        {
            var dtResult = new DataTable();
            try
            {
                using (var con = new SqlConnection(connectionStringnew))
                {
                    var cmd = new SqlCommand(sqlQuery, con);
                    var adp = new SqlDataAdapter(cmd);

                    adp.Fill(dtResult);

                    return dtResult;
                }
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
        }


        public static int? DeleteData(string table, string fieldname, string fieldvalue)
        {
            SqlCommand query = QueryBuilder.BuildDeleteQuery(table, fieldname, fieldvalue);
            using (SqlConnection connection = new SqlConnection(connectionStringnew))
            {
                SqlCommand cmd = query;
                cmd.Connection = connection;
                cmd.Connection.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static DataTable GetTable(string tableName, string where)
        {
            var dtResult = new DataTable();

            var query = !string.IsNullOrEmpty(where) ? $"select * from {tableName} readonly WITH (NOLOCK) Where {where}" : $"select * from {tableName} readonly WITH (NOLOCK)  ";

            using (var con = new SqlConnection(connectionStringnew))
            {
                var cmd = new SqlCommand(query, con);
                var adp = new SqlDataAdapter(cmd);

                adp.Fill(dtResult);

                return dtResult;
            }
        }

        public static DataTable ExecuteQuery(string Query)
        {
            var dtResult = new DataTable();

            var query = Query;

            using (var con = new SqlConnection(connectionStringnew))
            {
                var cmd = new SqlCommand(query, con);
                var adp = new SqlDataAdapter(cmd);

                adp.Fill(dtResult);

                return dtResult;
            }
        }

        public static bool ExecuteNonQuery(string query)
        {
            var success = false;

            using (SqlConnection con = new SqlConnection(connectionStringnew))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    success = true;
                }
            }
            return success;
        }

        public static DataTable ExecuteQueryTenant(string Query)
        {
            var dtResult = new DataTable();

            var query = Query;

            using (var con = new SqlConnection(tenantcnn))
            {
                var cmd = new SqlCommand(query, con);
                var adp = new SqlDataAdapter(cmd);

                adp.Fill(dtResult);

                return dtResult;
            }
        }

        public static bool IsExist(string query, bool tenantDb = false)
        {
            var retValue = false;
            var connectionString = connectionStringnew;

            if (tenantDb)
                connectionString = tenantcnn;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    var oValue = cmd.ExecuteScalar();

                    if (oValue != DBNull.Value && oValue != null)
                    {
                        retValue = true;
                    }
                    con.Close();
                }
            }
            return retValue;
        }


        public static DataTable GetTable(string TableName)
        {
            DataTable dtResult = new DataTable();
            try
            {
                string query = string.Empty;
                if (TableName != null)
                {
                    string[] splitTableName = TableName.Split('-');
                    if (splitTableName.Count() > 1)
                    {
                        query = "select * from " + splitTableName[0] + " (nolock) ";
                    }
                    else
                    {
                        query = "select * from " + TableName + " (nolock) ";
                    }
                }
                if (!string.IsNullOrWhiteSpace(query))
                {
                    string connectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["cnn"]);

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dtResult);
                        dtResult.TableName = TableName;

                    }

                }
                return dtResult;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
        }

        /// <summary>
        /// Method to return single value of a column from table, by ID. 
        /// </summary>
        public static object GetSingleValueByID(string tableName, string columnName, string ID, string TenantId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionStringnew))
                {
                    object value;
                    SqlCommand cmd = null;

                    cmd = new SqlCommand($"select {columnName} from {tableName} (nolock) where {DatabaseObjects.Columns.ID} = {ID} and {DatabaseObjects.Columns.TenantID} = '{TenantId}'", con);

                    con.Open();
                    value = cmd.ExecuteScalar();
                    con.Close();
                    return value;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
        }

        /// <summary>
        /// Method to return single value of a column from table, by TicketId. 
        /// </summary>
        public static object GetSingleValueByTicketId(string tableName, string columnName, string TicketId, string TenantId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionStringnew))
                {
                    object value;
                    SqlCommand cmd = null;

                    cmd = new SqlCommand($"select {columnName} from {tableName} (nolock) where {DatabaseObjects.Columns.TicketId} = '{TicketId}' and {DatabaseObjects.Columns.TenantID} = '{TenantId}'", con);

                    con.Open();
                    value = cmd.ExecuteScalar();
                    con.Close();
                    return value;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
        }

        /// <summary>
        /// Method to return selected columns from table. 
        /// </summary>
        public static DataTable GetTable(string tableName, string where, string columns)
        {
            var dtResult = new DataTable();

            var query = !string.IsNullOrEmpty(where) && !string.IsNullOrEmpty(columns) ? $"select {columns} from {tableName} (nolock) Where {where}" : $"select * from {tableName} (nolock)";

            using (var con = new SqlConnection(connectionStringnew))
            {
                var cmd = new SqlCommand(query, con);
                var adp = new SqlDataAdapter(cmd);

                adp.Fill(dtResult);

                return dtResult;
            }
        }

        /// <summary>
        /// Method to return table structure (Table without records).
        /// </summary>
        public static DataTable GetTableStructure(string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                var dtResult = new DataTable();

                var query = $"select top 0 * from {tableName} (nolock)";

                try
                {
                    using (var con = new SqlConnection(connectionStringnew))
                    {
                        var cmd = new SqlCommand(query, con);
                        var adp = new SqlDataAdapter(cmd);

                        adp.Fill(dtResult);

                        return dtResult;
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    return null;
                }
            }
            else
                return null;
        }

        public static List<Tenant> GetTenantList()
        {
            List<Tenant> listTenant = new List<Tenant>();

            var dtResult = (List<Tenant>)CacheHelper<object>.Get(string.Format("Available_Tenants"));
            if (dtResult == null)
            {
                CustomDbContext tcontext = new CustomDbContext(tenantcnn);
                using (DatabaseContext ctx = new DatabaseContext(tcontext))
                {
                    listTenant = ctx.Set<Tenant>().ToList();
                    CacheHelper<object>.AddOrUpdate(string.Format("Available_Tenants"), listTenant);
                }
            }
            else
            {
                listTenant = dtResult;
            }
            return listTenant;
        }

        public static long InsertItems<T>(List<T> obj) where T : class
        {
            int id = 0;
            //List<Tenant> listTenant = new List<Tenant>();
            //CustomDbContext tcontext = new CustomDbContext(tenantcnn);
            //using (DatabaseContext ctx = new DatabaseContext(tcontext))
            //{
            //    listTenant = ctx.Set<Tenant>().ToList();
            //}
            //   if (listTenant != null && listTenant.Count > 0)
            //listTenant.ForEach(y =>
            //{
            //obj.ForEach(x =>
            //{
            //    if (x.GetType().GetProperty("TenantID") != null)
            //        x.GetType().GetProperty("TenantID").SetValue(x, y.TenantID.ToString().ToLower(), null);
            //    if (x.GetType().GetProperty("ID") != null && (x.GetType().GetProperty("ID").PropertyType.Name == "Int32" || x.GetType().GetProperty("ID").PropertyType.Name == "Int64"))
            //        x.GetType().GetProperty("ID").SetValue(x, null, null);
            //   else if (x.GetType().GetProperty("Id") != null && (x.GetType().GetProperty("Id").PropertyType.Name == "Int32" || x.GetType().GetProperty("Id").PropertyType.Name == "Int64"))
            //        x.GetType().GetProperty("Id").SetValue(x, null, null);
            //   else if (x.GetType().GetProperty("id") != null && (x.GetType().GetProperty("id").PropertyType.Name == "Int32" || x.GetType().GetProperty("id").PropertyType.Name == "Int64"))
            //        x.GetType().GetProperty("id").SetValue(x, null, null);
            //});
            CustomDbContext context = new CustomDbContext(connectionStringnew);
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    ctx.Set<T>().AddRange(obj);
                    ctx.SaveChanges();

                    if (obj.Count > 0)
                        Console.WriteLine(string.Format("New Item {1} of {0} table is {0} created", id, obj.FirstOrDefault().GetType().Name));
                    else
                        Console.WriteLine("No items inserted");
                    id = 0;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    id = 0;
                }
            }
            //  });
            return id;
        }

        public static long InsertItem<T>(T obj) where T : class
        {
            long id = 0;
            //List<Tenant> listTenant = new List<Tenant>();
            //CustomDbContext tcontext = new CustomDbContext(tenantcnn);
            //using (DatabaseContext ctx = new DatabaseContext(tcontext))
            //{
            //    listTenant = ctx.Set<Tenant>().ToList();
            //}
            //if (listTenant != null && listTenant.Count > 0)
            //{
            //foreach (Tenant y in listTenant)
            //{
            //    //if (obj.GetType().GetProperty("TenantID") != null)
            //    obj.GetType().GetProperty("TenantID").SetValue(obj, y.TenantID.ToString().ToLower(), null);
            //if (obj.GetType().GetProperty("ID") != null && (obj.GetType().GetProperty("ID").PropertyType.Name == "Int32" || obj.GetType().GetProperty("ID").PropertyType.Name == "Int64"))
            //    obj.GetType().GetProperty("ID").SetValue(obj, null, null);
            //else if (obj.GetType().GetProperty("Id") != null && (obj.GetType().GetProperty("Id").PropertyType.Name == "Int32" || obj.GetType().GetProperty("Id").PropertyType.Name == "Int64"))
            //    obj.GetType().GetProperty("Id").SetValue(obj, null, null);
            //else if (obj.GetType().GetProperty("id") != null && (obj.GetType().GetProperty("id").PropertyType.Name == "Int32" || obj.GetType().GetProperty("ID").PropertyType.Name == "Int64"))
            //    obj.GetType().GetProperty("id").SetValue(obj, null, null);


            CustomDbContext context = new CustomDbContext(connectionStringnew);
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    ctx.Set<T>().Add(obj);
                    ctx.SaveChanges();
                    Console.WriteLine(string.Format("New Item of {0} table is created", obj.GetType().Name));
                    id = 1;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    id = 0;
                }
            }
            //  }
            //  }
            return id;
        }

        public static long InsertItem<T>(List<T> objs) where T : class
        {
            int id = 0;
            //List<Tenant> listTenant = new List<Tenant>();
            //CustomDbContext tcontext = new CustomDbContext(tenantcnn);
            //using (DatabaseContext ctx = new DatabaseContext(tcontext))
            //{
            //    listTenant = ctx.Set<Tenant>().ToList();
            //}
            // if (listTenant != null && listTenant.Count > 0)
            //foreach (Tenant y in listTenant)
            //{
            //foreach (object x in objs)
            //{
            //    if (x.GetType().GetProperty("TenantID") != null)
            //        x.GetType().GetProperty("TenantID").SetValue(x, y.TenantID.ToString().ToLower(), null);
            //    if (x.GetType().GetProperty("ID") != null && (x.GetType().GetProperty("ID").PropertyType.Name == "Int32"|| x.GetType().GetProperty("ID").PropertyType.Name == "Int64"))
            //        x.GetType().GetProperty("ID").SetValue(x, null, null);
            //    else if (x.GetType().GetProperty("Id") != null && (x.GetType().GetProperty("Id").PropertyType.Name == "Int32" || x.GetType().GetProperty("Id").PropertyType.Name == "Int64"))
            //        x.GetType().GetProperty("Id").SetValue(x, null, null);
            //    else if (x.GetType().GetProperty("id") != null && (x.GetType().GetProperty("id").PropertyType.Name == "Int32" || x.GetType().GetProperty("id").PropertyType.Name == "Int64"))
            //        x.GetType().GetProperty("id").SetValue(x, null, null);
            //}


            CustomDbContext context = new CustomDbContext(connectionStringnew);
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {

                    ctx.Set<T>().AddRange(objs);
                    ctx.SaveChanges();

                    if (objs.Count > 0)
                        Console.WriteLine(string.Format("New Item {1} of {0} table is {0} created", id, objs.FirstOrDefault().GetType().Name));
                    else
                        Console.WriteLine("No items inserted");
                    id = 0;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    id = 0;
                }
            }
            // }
            return id;
        }

        public static long InsertItemComman<T>(List<T> objs) where T : class
        {
            CustomDbContext context = new CustomDbContext(tenantcnn);
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    ctx.Set<T>().AddRange(objs);
                    ctx.SaveChanges();

                    if (objs.Count > 0)
                        Console.WriteLine(string.Format("New Item {1} of {0} table is {0} created", objs.Count, objs.FirstOrDefault().GetType().Name));
                    else
                        Console.WriteLine("No items inserted");

                    return 1;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    return 0;
                }
            }
        }

        public static long UpdateItemCommon<T>(T objs) where T : class
        {
            CustomDbContext context = new CustomDbContext(tenantcnn);
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    ctx.Set<T>().Update(objs);
                    ctx.SaveChanges();
                    return 1;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    return 0;
                }
            }
        }

        public static long InsertItemComman<T>(T obj) where T : class
        {
            CustomDbContext context = new CustomDbContext(tenantcnn);
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    ctx.Set<T>().Add(obj);
                    ctx.SaveChanges();

                    if (obj != null)
                        Console.WriteLine(string.Format("New Item {1} of {0} table is {0} created", obj, obj.GetType().Name));
                    else
                        Console.WriteLine("No items inserted");
                    return 1;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    Console.WriteLine("Account Id is already exists. Please enter another Account Id");
                    return 0;
                }
            }
        }

        public static bool Update<T>(T obj, string tableName) where T : class
        {
            CustomDbContext context = new CustomDbContext(connectionStringnew);
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                ctx.Set<T>().Update(obj);
                ctx.SaveChanges();
                bool updated = true;
                return updated;
            }
        }

        public static bool Delete<T>(T obj, string tableName) where T : class
        {
            CustomDbContext context = new CustomDbContext(connectionStringnew);
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                ctx.Set<T>().Remove(obj);
                int changes = ctx.SaveChanges();
                if (changes > 0)
                    return true;
                return false;
            }
        }



        public static bool IsLookaheadTicketExists(string moduleTable, string tenantId, string title, string requestTypeLookup, bool? rpaFlag)
        {
            var retValue = false;

            if (string.IsNullOrEmpty(title))
                return retValue;

            var query = string.Empty;
            title = title.Replace("'", "''");

            if (!string.IsNullOrEmpty(requestTypeLookup))
            {
                //With FREETEXT
                query = $"IF EXISTS (select top (1) 1 from Phrase where FREETEXT(Phrase, '{title}') and TenantID='{tenantId}') BEGIN select top (1) 1 from {moduleTable} where FREETEXT(Title, '{title}') and TenantID='{tenantId}' and RequestTypeLookup={requestTypeLookup} END";

                //With Like Query 
                // query = query = $"IF EXISTS (select top (1) 1 from Phrase where Phrase like '%{title}%' and TenantID='{tenantId}') BEGIN select top (1) 1 from {moduleTable} where Title like '%{title}%' and TenantID='{tenantId}' and RequestTypeLookup={requestTypeLookup}  END";
            }
            else if (rpaFlag == true)
            {
                query = query = $"IF EXISTS (select top (1) 1 from Phrase where Phrase like '%{title}%' and TenantID='{tenantId}') BEGIN select top (1) 1 from {moduleTable} where Title like '%{title}%' and TenantID='{tenantId}' END";
            }
            else
                query = $"IF EXISTS (select top (1) 1 from Phrase where FREETEXT(Phrase,'{title}') and TenantID='{tenantId}') BEGIN select top (1) 1 from {moduleTable} where FREETEXT(Title,'{title}') and TenantID='{tenantId}' END";
            //query = query = $"IF EXISTS (select top (1) 1 from Phrase where Phrase like '%{title}%' and TenantID='{tenantId}') BEGIN select top (1) 1 from {moduleTable} where Title like '%{title}%' and TenantID='{tenantId}' END";


            using (SqlConnection con = new SqlConnection(connectionStringnew))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    var oValue = cmd.ExecuteScalar();
                    if (oValue != DBNull.Value)
                    {
                        retValue = Convert.ToBoolean(oValue);
                    }
                    con.Close();
                }
            }
            return retValue;
        }

        public static bool IsResetPasswordExists(string tenantId, string title)
        {
            var retValue = false;

            if (string.IsNullOrEmpty(title))
                return retValue;

            var query = string.Empty;

            title = title.Replace("'", "''");
            query = $"(select top (1) 1 from Phrase where FREETEXT(Phrase,'{title}') and TenantID='{tenantId}' and AgentType = 1)";
            // query = $"(select top (1) 1 from Phrase where Phrase like '%{title}%' and TenantID='{tenantId}' and AgentType ='1')";
            using (SqlConnection con = new SqlConnection(connectionStringnew))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    var oValue = cmd.ExecuteScalar();
                    if (oValue != DBNull.Value)
                    {
                        retValue = Convert.ToBoolean(oValue);
                    }
                    con.Close();
                }
            }
            return retValue;
        }

        public static DataTable autoSetRequestor(string moduleTable, string tenantId, string title)
        {

            var query = string.Empty;
            query = $"Select * from {moduleTable} where  Title like '%{title}%' AND  TenantID='{tenantId}'";
            //using (SqlConnection con = new SqlConnection(connectionStringnew))
            //{
            //    using (SqlCommand cmd = new SqlCommand(query, con))
            //    {
            //        con.Open();
            //        var oValue = cmd.ExecuteScalar();
            //        if (oValue != DBNull.Value)
            //        {
            //            retValue = Convert.ToBoolean(oValue);
            //        }
            //        con.Close();
            //    }
            //}
            var dtResult = new DataTable();
            using (var con = new SqlConnection(connectionStringnew))
            {
                var cmd = new SqlCommand(query, con);
                var adp = new SqlDataAdapter(cmd);

                adp.Fill(dtResult);

                return dtResult;
            }


        }

        public static DataTable autoSetResolutionTime(string moduleTable, string tenantId, string title)
        {

            var query = string.Empty;
            title = title.Replace("'", "''");


            //query = $"select(COUNT(RequestTypeLookup)) As co, RequestTypeLookup from(Select  RequestTypeLookup, Requestor  From {moduleTable} where Title like '%{title}%' AND  TenantID = '{tenantId}') As S Group By RequestTypeLookup";

            query = $"Select  *   From {moduleTable} where FREETEXT(Title,'{title}') AND  TenantID = '{tenantId}' AND CLOSED = 1";

            var dtResult = new DataTable();
            using (var con = new SqlConnection(connectionStringnew))
            {
                var cmd = new SqlCommand(query, con);
                var adp = new SqlDataAdapter(cmd);

                adp.Fill(dtResult);

                return dtResult;
            }


        }

        public static DataTable autoSetRequestType(string moduleTable, string tenantId, string title)
        {

            var query = string.Empty;
            title = title.Replace("'", "''");


            //query = $"select(COUNT(RequestTypeLookup)) As co, RequestTypeLookup from(Select  RequestTypeLookup, Requestor  From {moduleTable} where Title like '%{title}%' AND  TenantID = '{tenantId}') As S Group By RequestTypeLookup";

            query = $"select(COUNT(RequestTypeLookup)) As co, RequestTypeLookup from(Select  RequestTypeLookup, RequestorUser  From {moduleTable} where FREETEXT(Title,'{title}') AND  TenantID = '{tenantId}' AND RequestTypeLookup  is not NULL AND RequestTypeLookup  != '') As S Group By RequestTypeLookup";

            var dtResult = new DataTable();
            using (var con = new SqlConnection(connectionStringnew))
            {
                var cmd = new SqlCommand(query, con);
                var adp = new SqlDataAdapter(cmd);

                adp.Fill(dtResult);

                return dtResult;
            }


        }




        public static DataTable autoSetPRP(string moduleTable, string tenantId, string title, string cloumn, int status, bool rpaFlag = false)
        {

            var query = string.Empty;
            if (rpaFlag)
            {
                query = $"select(COUNT({cloumn})) As co, {cloumn} from(Select  {cloumn}  From {moduleTable} where  Title like '%{title}%' AND  TenantID = '{tenantId}' AND CLOSED = {status} AND {cloumn}  is not NULL AND {cloumn}  != '' ) As S Group By {cloumn}";
            }
            else
                query = $"select(COUNT({cloumn})) As co, {cloumn} from(Select  {cloumn}  From {moduleTable} where FREETEXT(Title,'{title}') AND  TenantID = '{tenantId}' AND CLOSED = {status} AND {cloumn}  is not NULL AND {cloumn}  != '' ) As S Group By {cloumn}";
            //query = $"select(COUNT({cloumn})) As co, {cloumn} from(Select  {cloumn}  From {moduleTable} where  Title like '%{title}%' AND  TenantID = '{tenantId}' AND CLOSED = {status} AND {cloumn}  is not NULL AND {cloumn}  != '' ) As S Group By {cloumn}";

            //query = $"Select * from {moduleTable} where  Title like '%{title}%' AND  TenantID='{tenantId}'";
            //using (SqlConnection con = new SqlConnection(connectionStringnew))
            //{
            //    using (SqlCommand cmd = new SqlCommand(query, con))
            //    {
            //        con.Open();
            //        var oValue = cmd.ExecuteScalar();
            //        if (oValue != DBNull.Value)
            //        {
            //            retValue = Convert.ToBoolean(oValue);
            //        }
            //        con.Close();
            //    }
            //}
            var dtResult = new DataTable();
            using (var con = new SqlConnection(connectionStringnew))
            {
                var cmd = new SqlCommand(query, con);
                var adp = new SqlDataAdapter(cmd);

                adp.Fill(dtResult);

                return dtResult;
            }


        }

        public static DataTable autoSetPRP(string moduleTable, string tenantId, string title, string cloumn, string status)
        {
            var query = string.Empty;

            query = $"select(COUNT({cloumn})) As co, {cloumn} from(Select  {cloumn}  From {moduleTable} where  Title like '%{title}%' AND  TenantID = '{tenantId}' AND status = '{status}' AND {cloumn}  is not NULL AND {cloumn}  != '' ) As S Group By {cloumn}";

            var dtResult = new DataTable();
            using (var con = new SqlConnection(connectionStringnew))
            {
                var cmd = new SqlCommand(query, con);
                var adp = new SqlDataAdapter(cmd);

                adp.Fill(dtResult);

                return dtResult;
            }

        }

        public static bool IsAsset(string tenantId, string title, string tableName)
        {
            var retValue = false;

            var query = string.Empty;

            title = title.Replace("'", "''");
            query = $"(select top (1) 1 from {tableName} where title= '{title}' and TenantID='{tenantId}')";

            using (SqlConnection con = new SqlConnection(connectionStringnew))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    var oValue = cmd.ExecuteScalar();
                    if (oValue != DBNull.Value)
                    {
                        retValue = Convert.ToBoolean(oValue);
                    }
                    con.Close();
                }
            }
            return retValue;
        }


        public static DataTable GetTenantDataUsingQueries(string sqlQuery)
        {
            var dtResult = new DataTable();

            using (var con = new SqlConnection(tenantcnn))
            {
                var cmd = new SqlCommand(sqlQuery, con);
                var adp = new SqlDataAdapter(cmd);

                adp.Fill(dtResult);

                return dtResult;
            }
        }

        public static object GetSingleValue(string tableName, string where, string columns)
        {
            //var dtResult = new DataTable();
            object value;


            var query = !string.IsNullOrEmpty(where) && !string.IsNullOrEmpty(columns) ? $"select {columns} from {tableName} (nolock) Where {where}" : $"select * from {tableName} (nolock)";

            using (var con = new SqlConnection(connectionStringnew))
            {
                var cmd = new SqlCommand(query, con);

                // var adp = new SqlDataAdapter(cmd);

                // adp.Fill(dtResult);
                con.Open();

                value = cmd.ExecuteScalar();
                con.Close();
                return value;
            }
        }
        public static bool bulkupload(DataTable sourcetable, string destinationtable)
        {
            var retValue = false;
            using (var bulkCopy = new SqlBulkCopy(connectionStringnew, SqlBulkCopyOptions.KeepIdentity))
            {
                // my DataTable column names match my SQL Column names, so I simply made this loop. However if your column names don't match, just pass in which datatable name matches the SQL column name in Column Mappings
                foreach (DataColumn col in sourcetable.Columns)
                {
                    if (col.ColumnName != "ID")
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                }
                bulkCopy.BulkCopyTimeout = 1000;
                bulkCopy.DestinationTableName = destinationtable;
                bulkCopy.WriteToServer(sourcetable);
                retValue = true;
            }
            return retValue;
        }

        public static DataTable GetTableSchema(string connectionString, string tableName)
        {
            var dtResult = new DataTable();

            using (var con = new SqlConnection(connectionString))
            {
                string sqlQuery = string.Format("select * from information_schema.COLUMNS where TABLE_NAME = '{0}'", tableName);
                var cmd = new SqlCommand(sqlQuery, con);
                var adp = new SqlDataAdapter(cmd);

                adp.Fill(dtResult);
                return dtResult;
            }
        }
        public static DataTable ExecuteDataSetWithParameters(string StoredProcName, Dictionary<string, object> values)
        {
            // This is the function to execute any stored procedure with Parameter and gets the resultset / Dataset
            SqlConnection Conn;
            Conn = new SqlConnection(connectionStringnew);
            DataTable DT = new DataTable();
            try
            {
                SqlCommand Cmd = new SqlCommand();
                SqlDataAdapter DA;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = StoredProcName;
                Cmd.CommandTimeout = 3600;
                Cmd.Connection = Conn;
                // Opening The Connection
                Conn.Open();
                foreach (KeyValuePair<string, object> keyValuePair in values)
                {
                    Cmd.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value == null ? "" : keyValuePair.Value.ToString());
                }
                DA = new SqlDataAdapter(Cmd);
                DA.Fill(DT);
                Cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, $"StoredProcedure: {StoredProcName}");
            }
            finally
            {
                Conn.Close();
            }
            return DT;
        }
        public static DataSet ExecuteDataSet_WithParameters(string StoredProcName, Dictionary<string, object> values)
        {
            // This is the function to execute any stored procedure with Parameter and gets the resultset / Dataset
            SqlConnection Conn;
            Conn = new SqlConnection(connectionStringnew);
            DataSet DS = new DataSet();
            try
            {
                SqlCommand Cmd = new SqlCommand();
                SqlDataAdapter DA;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandTimeout = 0;
                Cmd.CommandText = StoredProcName;
                Cmd.Connection = Conn;
                // Opening The Connection
                Conn.Open();
                foreach (KeyValuePair<string, object> keyValuePair in values)
                {
                    Cmd.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value == null ? "" : keyValuePair.Value.ToString());
                }
                DA = new SqlDataAdapter(Cmd);
                DA.Fill(DS);
                Cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, $"StoredProcedure: {StoredProcName}");
            }
            finally
            {
                Conn.Close();
            }
            return DS;
        }
        public static string CreateTempTable()
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionStringnew);
                SqlCommand cmd = new SqlCommand("IF NOT EXISTS  (SELECT [name]  FROM sys.tables  WHERE[name] = 'MyTempTable') begin create table MyTempTable(SPId varchar(max), DId varchar(max),TenantId varchar(max) ,TicketId varchar(max),ModuleId varchar(max)) end", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return "MyTempTable";
        }
        public static int ExecuteNonQueryWithParameters(string StoredProcName, Dictionary<string, object> values)
        {
            // This is the function to execute any stored procedure with Parameter
            string errMsg1;
            int output = 0;
            SqlConnection Conn;
            Conn = new SqlConnection(connectionStringnew);
            SqlCommand Cmd;
            Cmd = new SqlCommand(StoredProcName, Conn);
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Connection = Conn;
            try
            {
                // Opening The Connection
                Conn.Open();
                foreach (KeyValuePair<string, object> keyValuePair in values)
                {
                    Cmd.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value == null ? "" : keyValuePair.Value.ToString());
                }
                // Executing The Stored Procedure
               output= Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errMsg1 = ex.Message;
                ULog.WriteException(ex, $"StoredProcedure: {StoredProcName}");
            }
            finally
            {
                Conn.Close();
            }
            return output;
        }
    }
}
