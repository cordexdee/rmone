using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Configuration;

namespace ITAnalyticsBL.ET
{
    public class ETContext 
    {
        private static Object lockObject = new Object();
        public const string TABLEFORMAT = "ETFactTable_{0}_{1}";
        // <summary>
        // Create fact tables
        // </summary>
        // <param name="tableSchema"></param>
        // <returns>return msg code: 0 for already exist, 1 for created</returns>
        public static int CreateTable(DataTable tableSchema)
        {
            int messageCode = 0;

            SqlConnection conn = GetDBConnection();
            SqlCommand commnd = null;
            try
            {
                StringBuilder queryString = new System.Text.StringBuilder();
                queryString.AppendFormat("CREATE TABLE [dbo].[{0}](", tableSchema.TableName);
                commnd = new SqlCommand(string.Format("select * from information_schema.tables where table_name ='{0}'", tableSchema.TableName), conn);
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(commnd);
                DataSet resultedSet = new DataSet();
                adapter.Fill(resultedSet);
                conn.Close();
                if (resultedSet == null || (resultedSet.Tables.Count > 0 && resultedSet.Tables[0].Rows.Count <= 0))
                {
                    foreach (DataColumn column in tableSchema.Columns)
                    {
                        if (tableSchema.Columns.IndexOf(column) != 0)
                        {
                            queryString.Append(",");
                        }

                            if (column.DataType == typeof(string))
                            {

                                if(Array.IndexOf(tableSchema.PrimaryKey, column) >= 0)// check if data table contains any primary key as nvarchar(max) is not a valid datatype for primary key.
                                {
                                    queryString.AppendFormat("[{0}] [nvarchar](100) NOT NULL default('')", column.ColumnName);
                                }
                                else
                                {
                                    queryString.AppendFormat("[{0}] [nvarchar](max) NOT NULL default('')", column.ColumnName);
                                }
                            }
                            else if (column.DataType == typeof(DateTime))
                            {
                                queryString.AppendFormat("[{0}] datetime NULL", column.ColumnName);
                            }
                            else
                            {
                                queryString.AppendFormat("[{0}] decimal(10,2) NOT NULL default(0)", column.ColumnName);
                            }

                    }
                    // if Data table contains primary key then it creats the primary key constraint and create the primary key in the table.
                    if (tableSchema.PrimaryKey.Length > 0)
                    {
                        queryString.AppendFormat("CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED (", tableSchema);

                        foreach (DataColumn column in tableSchema.PrimaryKey)
                        {
                            queryString.AppendFormat("[{0}],", column.ColumnName);
                        }
                        queryString.Remove(queryString.Length - 1, 1); // removes the extra comma from the string.
                        queryString.Append(")");
                    }


                    queryString.Append(")");
                    conn.Open();
                    commnd = new SqlCommand(queryString.ToString(), conn);
                    int result = commnd.ExecuteNonQuery();
                    messageCode = 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (commnd != null)
                {
                    commnd.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
            return messageCode;
        }

        private static SqlConnection GetDBConnection()
        {
            try
            {
                ConnectionStringSettings connectionSetting = ConfigurationManager.ConnectionStrings["ETContext"];
                string connectionString = connectionSetting.ConnectionString;
                return new SqlConnection(connectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static int FillData(DataTable table)
        {
            //lock (lockObject)
            //{
                int messageCode = 0;

                SqlConnection conn = GetDBConnection();
                SqlCommand commnd = null;
                try
                {
                    commnd = new SqlCommand(string.Format("select * from [{0}]", table.TableName), conn);
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(commnd);
                    DataSet resultedSet = new DataSet();

                    adapter.Fill(resultedSet);

                    conn.Close();



                    if (resultedSet != null && resultedSet.Tables.Count > 0)
                    {
                        conn.Open();
                        SqlCommandBuilder comdBuilder = new SqlCommandBuilder(adapter);
                        // resultedSet.AcceptChanges();
                        comdBuilder.ConflictOption = ConflictOption.OverwriteChanges;
                       
                        //adapter.AcceptChangesDuringUpdate = true;
                        //adapter.Fill(resultedSet);
                        //adapter.UpdateCommand = comdBuilder.GetUpdateCommand();
                        //DataSet ds = new DataSet();
                        //adapter.Fill(ds);
                        
                        adapter.Update(table.Select(null,null,DataViewRowState.Deleted));
                        adapter.Update(table.Select(null,null,DataViewRowState.ModifiedCurrent));
                        adapter.Update(table.Select(null, null, DataViewRowState.Added));
                        //adapter.ContinueUpdateOnError = true;
                        //adapter.Update(table);
                        //resultedSet.EndInit();
                        //adapter.Update(resultedSet);
                        //adapter.Fill(resultedSet);
                        conn.Close();
                    }

                }

                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (commnd != null)
                    {
                        commnd.Dispose();
                    }
                    conn.Close();
                    conn.Dispose();
                }
                return messageCode;
            //}
        }
        // <summary>
        // returns the schema of the table
        // </summary>
        // <param name="ETTable"></param>
        // <returns></returns>

        public static DataTable  GetTableSchema(string ETTable)
        {
            SqlConnection conn = GetDBConnection();
            SqlCommand cmd = null;
            
            try
            {
                cmd = new SqlCommand(string.Format("select * from information_schema.COLUMNS where TABLE_NAME = '{0}'", ETTable), conn);
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
            
        }
        // <summary>
        // Returns all the selected columns value of a particular table
        // </summary>
        // <param name="etTable"></param>
        // <param name="column"></param>
        // <returns></returns>
        public static DataTable GetAssociatedColumnValues(String etTable, params string[] column)
        {
            SqlConnection conn = GetDBConnection();
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;

            try
            {
                for (int i = 0; i < column.Length; i++)
                {
                    if (i >= 0)
                    {
                        {
                            cmd = new SqlCommand(string.Format("Select [{0}] from [{1}]", column[i], etTable), conn);
                        }

                    }
                }

                    conn.Open();
                    dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                
            }
            catch (Exception ex)
            {
                
               throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
        }

        // <summary>
        // Get filtered value base specified filters
        // </summary>
        // <param name="etTable"></param>
        // <param name="column"></param>
        // <returns></returns>
        public static DataTable GetDatatableByCriteria(string tenantID, string etTable, string column = null, string whereClause = null)
        {
            SqlConnection conn = GetDBConnection();
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;

            etTable = string.Format(ETContext.TABLEFORMAT, tenantID, etTable);
            try
            {
                if (!string.IsNullOrEmpty(etTable))
                {
                    if (string.IsNullOrWhiteSpace(column))
                        column = "*";

                    if (!string.IsNullOrWhiteSpace(whereClause))
                        whereClause = $"where {whereClause}";

                    cmd = new SqlCommand(string.Format("Select {1} from [{0}] {2}", etTable, column, whereClause), conn);

                    conn.Open();
                    dt = new DataTable();
                    
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                   
                }
                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
        }
        public static Boolean IsTableExist(string etTable)
        {
            Boolean messageCode = false;

            SqlConnection conn = GetDBConnection();
            SqlCommand commnd = null;
            try
            {
                commnd = new SqlCommand(string.Format(" select * from sysobjects where xtype = 'u' and name = '{0}'", etTable), conn);
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(commnd);
                DataSet resultedSet = new DataSet();

                adapter.Fill(resultedSet);
                if (resultedSet != null && resultedSet.Tables[0].Rows.Count>0)
                {
                    messageCode = true;
                }
               

            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (commnd != null)
                {
                    commnd.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
            return messageCode;
        }

        public static int DeleteData(string etTable)
        {
            int messageCode = 0;

            SqlConnection conn = GetDBConnection();
            SqlCommand commnd = null;
            //if(ETContext.IsTableExist(etTable))
            //{
                try
                {
                    commnd = new SqlCommand(string.Format("delete  from [{0}]", etTable), conn);
                    conn.Open();
                    messageCode = commnd.ExecuteNonQuery();

                }

                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (commnd != null)
                    {
                        commnd.Dispose();
                    }
                    conn.Close();
                    conn.Dispose();
                }
            //}
            return messageCode;

          }
        //public void test()
        //{
        //    // Establish the database server
        //    string connectionString = "...";
        //    SqlConnection connection =
        //         new SqlConnection(connectionString);

        //    Server server = new Server(new ServerConnection(connection));

        //    // Create table in my personal database
        //    Database db = server.Databases["davidhayden"];

        //    // Create new table, called TestTable
        //    Table newTable = new Table(db, "TestTable");

        //    // Add "ID" Column, which will be PK
        //    Column idColumn = new Column(newTable, "ID");
        //    idColumn.DataType = DataType.Int;
        //    idColumn.Nullable = false;
        //    idColumn.Identity = true;
        //    idColumn.IdentitySeed = 1;
        //    idColumn.IdentityIncrement = 1;

        //    // Add "Title" Column
        //    Column titleColumn = new Column(newTable, "Title");
        //    titleColumn.DataType = DataType.VarChar(50);
        //    titleColumn.Nullable = false;

        //    // Add Columns to Table Object
        //    newTable.Columns.Add(idColumn);
        //    newTable.Columns.Add(titleColumn);

        //    // Create a PK Index for the table
        //    Index index = new Index(newTable, "PK_TestTable");
        //    index.IndexKeyType = IndexKeyType.DriPrimaryKey;

        //    // The PK index will consist of 1 column, "ID"
        //    index.IndexedColumns.Add(new IndexedColumn(index, "ID"));

        //    // Add the new index to the table.
        //    newTable.Indexes.Add(index);

        //    // Physically create the table in the database
        //    newTable.Create();
        //}

    }
}
