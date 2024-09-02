using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using uGovernIT.Utility;
using System.Collections;
using uGovernIT.Util.Log;

namespace uGovernIT.DAL
{
    public class TicketDal
    {
        private CustomDbContext context;

        public TicketDal(CustomDbContext ctx)
        {
            context = ctx;
        }

        public static DataTable GetTickets(string _query)
        {
            string connectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["cnn"]);
            DataTable dtResult = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(_query, con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dtResult);
                return dtResult;
            }
        }
      
     
        public static DataTable SaveTickettemp(DataRow row, string tableName, bool isNewTicket)
        {

            string connectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["cnn"]);
            DataTable dt = new DataTable();
            dt.TableName = tableName;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string sql = "select * from " + tableName + "(nolock)";
                    if (!isNewTicket)
                    {
                        sql += " where " + DatabaseObjects.Columns.ID + "=" + Convert.ToString(row[DatabaseObjects.Columns.ID]);
                    }
                    SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                    adp.Fill(dt);
                    if (isNewTicket)
                    {
                        if (UGITUtility.IsSPItemExist(row, DatabaseObjects.Columns.IsPrivate))
                        {
                            if (string.IsNullOrEmpty(row["isPrivate"].ToString()))
                            {
                                row["isPrivate"] = true;
                            }
                        }
                        dt.Rows.Add(row.ItemArray);
                    }
                    else
                    {
                        dt.Select(DatabaseObjects.Columns.ID + "=" + row[DatabaseObjects.Columns.ID]).Single().ItemArray = row.ItemArray;
                    }
                    SqlCommandBuilder builder = new SqlCommandBuilder(adp);

                    adp.UpdateCommand = builder.GetUpdateCommand(true);
                    adp.Update(dt);
                    dt.AcceptChanges();

                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            return dt;
            // return uGITDAL.GetTable(tableName, DatabaseObjects.Columns.TicketId+"='" + row[DatabaseObjects.Columns.TicketId].ToString()+"'");
        }
        public static DataTable SaveTicket(DataRow row, string tableName, bool isNewTicket)
        {

            string connectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["cnn"]);

            DataTable dt = new DataTable();

            dt.TableName = tableName;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string sql = "select * from " + tableName + " (nolock)";
                    if (!isNewTicket)
                    {
                        sql += " where " + DatabaseObjects.Columns.ID + "=" + Convert.ToString(row[DatabaseObjects.Columns.ID]);
                    }
                    SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                    adp.Fill(dt);
                    if (isNewTicket)
                    {
                        if (UGITUtility.IsSPItemExist(row, DatabaseObjects.Columns.IsPrivate))
                        {
                            if (string.IsNullOrEmpty(row["isPrivate"].ToString()))
                            {
                                row["isPrivate"] = true;
                            }
                        }
                        dt.Rows.Add(row.ItemArray);
                    }
                    
                    else
                    {
                        dt.Select(DatabaseObjects.Columns.ID + "=" + row[DatabaseObjects.Columns.ID]).Single().ItemArray = row.ItemArray;
                    }
                    SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                    adp.UpdateCommand = builder.GetUpdateCommand(true);
                    adp.Update(dt);
                    dt.AcceptChanges();

                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            //return dt;
            return uGITDAL.GetTable(tableName, DatabaseObjects.Columns.TicketId + "='" + row[DatabaseObjects.Columns.TicketId].ToString() + "'");
        }

        public static DataTable GetLookUpData(string tableName, string fieldName, int Id)
        {
            string _query = "Select ID, " + fieldName + " from " + tableName + " Where ID = " + Id;
            string connectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["cnn"]);
            DataTable dtResult = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(_query, con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dtResult);
            }
            return dtResult;

        }
        public static DataTable GetLookupValueCollectionData(string fieldName, string tableName, string Id)
        {
            DataTable dtResult = new DataTable();
            string[] arrayIds = UGITUtility.SplitString(Id, Constants.Separator).Distinct().ToArray();
            if (arrayIds.Length == 0 || string.IsNullOrEmpty(arrayIds[0]))
                return dtResult;

            string _query = string.Empty;
            string Ids = String.Join(",", arrayIds);
            _query = "Select ID, " + fieldName + " from" + tableName + "Where ID IN" + Ids;
            string connectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["cnn"]);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(_query, con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dtResult);

            }
            return dtResult;
        }

        public static DataTable GetTableBasedOnWhere(string tableName, string where)
        {
            string query = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(where))
                {
                    query = "Select * from " + tableName + " (nolock) " + where;
                }
                else
                {
                    query = "Select * from " + tableName + " (nolock) ";
                }
                return GetTickets(query);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return new DataTable();
            }
        }

        public int SaveHistory(string tableName, DataRow row)
        {
            if (row.RowState == DataRowState.Unchanged || row.RowState == DataRowState.Detached)
            {
                return -1;
            }

            using (SqlConnection con = new SqlConnection(context.Database))
            {
                int effectedRows = 0;

                SqlDataAdapter adp = new SqlDataAdapter(string.Format("select * from {0} where {1} = '{2}' and {1} is not null", tableName, DatabaseObjects.Columns.TenantID, context.TenantID), con);
                adp.UpdateBatchSize = 1;
                SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                builder.ConflictOption = ConflictOption.OverwriteChanges;
                adp.RowUpdated += Adp_RowUpdated;

                if (row.RowState == DataRowState.Added)
                {
                    row[DatabaseObjects.Columns.Created] = DateTime.Now;
                    row[DatabaseObjects.Columns.Modified] = DateTime.Now;
                    row[DatabaseObjects.Columns.TenantID] = context.TenantID;

                    if (context.CurrentUser != null)
                    {
                        row[DatabaseObjects.Columns.ModifiedByUser] = context.CurrentUser.Id;
                        row[DatabaseObjects.Columns.CreatedByUser] = context.CurrentUser.Id;
                        row[DatabaseObjects.Columns.TenantID] = context.CurrentUser.TenantID;
                    }
                    // Check if column exists in table
                    //if(UGITUtility.IsSPItemExist(row, DatabaseObjects.Columns.Deleted))
                    if (row.Table.Columns.Contains(DatabaseObjects.Columns.Deleted))
                        row[DatabaseObjects.Columns.Deleted] = false;

                    // Check if column exists in table
                    //if (row.Table.Columns.Contains(DatabaseObjects.Columns.IsDeletedColumn))

                    //    row[DatabaseObjects.Columns.IsDeletedColumn] = false;

                    //SLADisabled is set to false to fix issue relate to PRS module                    
                    if (row.Table.Columns.Contains(DatabaseObjects.Columns.SLADisabled))
                    {
                        row[DatabaseObjects.Columns.SLADisabled] = false;
                    }

                    DataRow[] insertRows = new DataRow[] { row };
                    SqlCommand insertCmd = builder.GetInsertCommand(true).Clone();
                    insertCmd.CommandText += string.Format(" SET @{0} = SCOPE_IDENTITY()", DatabaseObjects.Columns.ID);

                    // the SET command writes to an output parameter "@ID"
                    SqlParameter parm = new SqlParameter();
                    parm.Direction = ParameterDirection.Output;
                    parm.Size = 8;
                    parm.SqlDbType = SqlDbType.BigInt;
                    parm.ParameterName = string.Format("@{0}", DatabaseObjects.Columns.ID);
                    parm.DbType = DbType.Int64;
                    insertCmd.Parameters.Add(parm);

                    adp.InsertCommand = insertCmd;
                    builder.Dispose();
                    try
                    {
                        effectedRows = adp.Update(insertRows);
                   

                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex,"PMMHistory BaseLine");
                    }
                }
                //else if (row.RowState == DataRowState.Modified)
                //{
                //    row[DatabaseObjects.Columns.Modified] = DateTime.Now;
                //    if (context.CurrentUser != null)
                //    {
                //        row[DatabaseObjects.Columns.ModifiedByUser] = context.CurrentUser.Id;
                //        row[DatabaseObjects.Columns.TenantID] = context.CurrentUser.TenantID;
                //    }

                //    DataRow[] updatedRows = new DataRow[] { row };
                //    adp.UpdateCommand = builder.GetUpdateCommand(true);
                //    effectedRows = adp.Update(updatedRows);
                //}
                if (effectedRows > 0)
                {
                    row.AcceptChanges();

                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void Adp_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.StatementType == StatementType.Insert)
            {
                object id = e.Command.Parameters[string.Format("@{0}", DatabaseObjects.Columns.ID)].Value;
                e.Row[DatabaseObjects.Columns.ID] = id;
                e.Row.AcceptChanges();
            }

        }


    }
}
