using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public interface ITicketStore
    {
        //Added the viewfield to return only specific columns 
        //Anand
        DataTable GetOpenTickets(UGITModule module, ModuleStatisticRequest moduleRequest, List<string> viewFields = null);
        DataTable GetClosedTickets(UGITModule module, List<string> viewFields = null);
        DataTable GetCachedModuleTableSchema(UGITModule module);
        int Save(UGITModule module, DataRow row);
        int SaveArchive(UGITModule module, DataRow row);
        int Delete(UGITModule module, DataRow row);
        DataRow GetByTicketID(UGITModule module, string ticketID, List<string> viewFields = null);
        DataTable GetByTicketIDs(UGITModule module, List<string> ticketIDs, List<string> viewFields = null, bool includeDefaultColumnsForSave = false);
        DataRow GetByID(UGITModule module, long ID, List<string> viewFields = null);
        DataTable GetTickets(string query);
        DataTable GetTickets(string table, string where);

        void UpdateTicketCache(DataRow currentTicket, UGITModule moduleName, bool archive);
    }

    public class TicketStore : ITicketStore
    {
        private CustomDbContext context;

        public TicketStore(CustomDbContext ctx)
        {
            context = ctx;
        }

        public DataTable GetOpenTickets(UGITModule module, ModuleStatisticRequest moduleRequest = null, List<string> viewFields = null)
        {
            try
            {
                DataTable dataResult;
                string query;
                string fields = "*";

                if (viewFields != null && viewFields.Count > 0)
                    fields = string.Join(",", viewFields);

                if (moduleRequest != null && moduleRequest.Lookahead && !string.IsNullOrEmpty(moduleRequest.Title))
                {
                    var sessionTicketID = (List<string>)HttpContext.Current.Session["relatedTicket"];
                    DataTable dataResult1 = null;

                    if (sessionTicketID != null && sessionTicketID.Count > 0 && !string.IsNullOrEmpty(moduleRequest.RequestType))
                    {
                        var query1 = $"select {fields} from {module.ModuleTable} (nolock) where ({DatabaseObjects.Columns.TicketClosed} <> 1 or Closed is null) and ({DatabaseObjects.Columns.Deleted}<>1 or Deleted is null)and  {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.TenantID} is not null and TicketId in ('{string.Join("','", sessionTicketID)}')";
                        dataResult1 = GetTickets(query1);

                    }

                    if (string.IsNullOrEmpty(moduleRequest.RequestType))
                    {
                        query = $"select {fields} from {module.ModuleTable} (nolock) where ({DatabaseObjects.Columns.TicketClosed} <> 1 or Closed is null) and ({DatabaseObjects.Columns.Deleted}<>1 or Deleted is null) and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.TenantID} is not null and FREETEXT (Title,'{moduleRequest.Title.Trim()}')";
                    }
                    else
                    {
                        query = $"select {fields} from {module.ModuleTable} (nolock) where ({DatabaseObjects.Columns.TicketClosed} <> 1 or Closed is null) and ({DatabaseObjects.Columns.Deleted}<>1 or Deleted is null)and {DatabaseObjects.Columns.TicketRequestTypeLookup}={moduleRequest.RequestType}  and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.TenantID} is not null and FREETEXT(Title,'{moduleRequest.Title.Trim()}')";
                    }

                    dataResult = GetTickets(query);

                    if (sessionTicketID != null && sessionTicketID.Count > 0 && !string.IsNullOrEmpty(moduleRequest.RequestType))
                    {
                        var query1 = $"select {fields} from {module.ModuleTable} (nolock) where ({DatabaseObjects.Columns.TicketClosed} <> 1 or Closed is null) and ({DatabaseObjects.Columns.Deleted}<>1 or Deleted is null)and  {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.TenantID} is not null and TicketId in ('{string.Join("','", sessionTicketID)}')";
                        dataResult1 = GetTickets(query1);
                        dataResult.Merge(dataResult1);
                        dataResult.DefaultView.ToTable(true, "TicketId");
                    }
                }
                else
                {
                    if (viewFields != null)
                    {
                        query = string.Format("select {6} from {0}(nolock) where ({1} is null or {1} <> {3}) and ({2}<>{3} or Deleted is null) and {4} = '{5}' and {4} is not null", module.ModuleTable, DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.Deleted, 1, DatabaseObjects.Columns.TenantID, context.TenantID, fields);
                        dataResult = GetTickets(query);
                    }
                    else
                    {
                        dataResult = (DataTable)CacheHelper<object>.Get($"OpenTicket_{module.ModuleName}", context.TenantID);

                        if ((dataResult == null || dataResult.Rows.Count == 0) && !string.IsNullOrEmpty(module.ModuleTable))
                        {
                            Dictionary<string, object> values = new Dictionary<string, object>();
                            values.Add("@TenantID", context.TenantID);
                            values.Add("@ModuleName", module.ModuleName);
                            values.Add("@IsClosed", 0);
                            dataResult = uGITDAL.ExecuteDataSetWithParameters("USP_GetModuleTableData", values);
                            CacheHelper<object>.AddOrUpdate($"OpenTicket_{module.ModuleName}", context.TenantID, dataResult);
                            //query = string.Format("select {6} from {0} (nolock) where ({1} is null or {1} <> {3}) and ({2}<>{3} or Deleted is null) and {4} = '{5}' and {4} is not null", module.ModuleTable, DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.Deleted, 1, DatabaseObjects.Columns.TenantID, context.TenantID, fields);
                            //dataResult = GetTickets(query);
                            //CacheHelper<object>.AddOrUpdate($"OpenTicket_{module.ModuleName}", context.TenantID, dataResult);
                        }
                    }
                }

                try
                {
                    return dataResult?.Copy() ?? null;
                }
                catch (Exception ex)
                {
                    ULog.WriteException("Issue while sending GetOpenTickets >> " + ex.ToString());
                    return null;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
        }

        public DataTable GetClosedTickets(UGITModule module, List<string> viewFields = null)
        {
            try
            {
                var dtResult = new DataTable();
                string fields = "*";
                if (viewFields != null && viewFields.Count > 0)
                    fields = string.Join(",", viewFields);

                if (module != null)
                {
                    if (viewFields != null)
                    {
                        var query = string.Format("select {5} from {0} where {1} = {2} and {3} = '{4}' and {3} is not null", module.ModuleTable, DatabaseObjects.Columns.TicketClosed, 1, DatabaseObjects.Columns.TenantID, context.TenantID, fields);
                        dtResult = GetTickets(query);
                    }
                    else
                    {
                        dtResult = (DataTable)CacheHelper<object>.Get($"ClosedTicket_{module.ModuleName}", context.TenantID);

                        if (dtResult == null && !string.IsNullOrEmpty(module.ModuleTable))
                        {
                            Dictionary<string, object> values = new Dictionary<string, object>();
                            values.Add("@TenantID", context.TenantID);
                            values.Add("@ModuleName", module.ModuleName);
                            values.Add("@IsClosed", 1);
                            dtResult = uGITDAL.ExecuteDataSetWithParameters("USP_GetModuleTableData", values);
                            CacheHelper<object>.AddOrUpdate($"ClosedTicket_{module.ModuleName}", context.TenantID, dtResult);
                            //var query = string.Format("select {5} from {0} where {1} = {2} and {3} = '{4}' and {3} is not null", module.ModuleTable, DatabaseObjects.Columns.TicketClosed, 1, DatabaseObjects.Columns.TenantID, context.TenantID, fields);
                            //dtResult = GetTickets(query);
                            //CacheHelper<object>.AddOrUpdate($"ClosedTicket_{module.ModuleName}", context.TenantID, dtResult);
                        }
                    }

                }

                try
                {
                    return dtResult?.Copy() ?? null;
                }
                catch (Exception ex)
                {
                    ULog.WriteException("Issue while sending GetClosedTickets >> " + ex.ToString());
                    return null;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
        }

        public long GetAllTicketsCount(string Module)
        {
            try
            {
                if (!string.IsNullOrEmpty(Module))
                {
                    string query = string.Format($"select count(*) from {Module} where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.TenantID} is not null");
                    long count = Convert.ToInt64(GetTickets(query).Rows[0][0]);
                    return count;
                }
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            return 0;
        }


        public DataRow GetByTicketID(UGITModule module, string ticketID, List<string> viewFields = null)
        {
            DataRow dtResult = null;
            using (SqlConnection con = new SqlConnection(context.Database))
            {
                try
                {
                    string fields = "*";
                    if (viewFields != null && viewFields.Count > 0)
                        fields = string.Join(",", viewFields);

                    //SqlCommand cmd = new SqlCommand(string.Format("Select TOP 1 * from {0} where {1}='{2}'",module.ModuleTable, DatabaseObjects.Columns.TicketId, ticketID), con);
                    SqlCommand cmd = new SqlCommand(string.Format("Select TOP 1 {5} from {0} where {1}='{2}' and {3} = '{4}' and {3} is not null", module.ModuleTable, DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.TenantID, context.TenantID, fields), con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    adp.Fill(data);
                    if (data != null && data.Rows.Count > 0)
                    {
                        dtResult = data.Rows[0];
                        return dtResult;
                    }
                    else
                    {
                        return null;
                    }
                }catch(Exception ex)
                {
                    ULog.WriteException(ex);
                    return null;
                }
                
            }
        }

        public DataTable GetByTicketIDs(UGITModule module, List<string> ticketIDs, List<string> viewFields = null, bool includeDefaultColumnsForSave = false)
        {
            DataTable dtResult = null;
            using (SqlConnection con = new SqlConnection(context.Database))
            {
                try
                {
                    string fields = "*";
                    if (viewFields != null && viewFields.Count > 0)
                    {
                        if (includeDefaultColumnsForSave)
                        {
                            viewFields.AddRange(new List<string> 
                            {
                                DatabaseObjects.Columns.ID,
                                DatabaseObjects.Columns.TenantID,
                                DatabaseObjects.Columns.Created,
                                DatabaseObjects.Columns.Modified,
                                DatabaseObjects.Columns.CreatedByUser,
                                DatabaseObjects.Columns.ModifiedByUser,
                                DatabaseObjects.Columns.Deleted,
                            });
                        }
                        fields = string.Join(",", viewFields.Distinct());
                    }
                    //SqlCommand cmd = new SqlCommand(string.Format("Select TOP 1 * from {0} where {1}='{2}'",module.ModuleTable, DatabaseObjects.Columns.TicketId, ticketID), con);
                    SqlCommand cmd = new SqlCommand(
                        string.Format("Select {5} from {0} where {1} IN ({2}) and {3} = '{4}' and {3} is not null",
                        module.ModuleTable, DatabaseObjects.Columns.TicketId, string.Join("," , ticketIDs.Select(t=> $"'{t}'")), DatabaseObjects.Columns.TenantID, context.TenantID, fields), con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    dtResult = new DataTable();
                    adp.Fill(dtResult);
                    return dtResult;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }

            }
            return dtResult;
        }


        /// <summary>
        /// Method to return single value of a column from table, Ticket ID. 
        /// </summary>
        public object GetSingleValueByTicketID(string tableName, string columnName, string ticketID)
        {
            using (SqlConnection con = new SqlConnection(context.Database))
            {
                try
                {
                    object value;
                    SqlCommand cmd = null;
                    if (!string.IsNullOrEmpty(columnName))
                        cmd = new SqlCommand($"select {columnName} from {tableName} (nolock) where {DatabaseObjects.Columns.TicketId} = '{ticketID}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'", con);
                    else
                        cmd = new SqlCommand($"select * from {tableName} (nolock) where {DatabaseObjects.Columns.TicketId} = '{ticketID}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'", con);

                    con.Open();
                    value = cmd.ExecuteScalar();
                    con.Close();
                    return value;
                }catch(Exception ex)
                {
                    ULog.WriteException (ex);   
                    return null;
                }
            }
        }
        public object GetIDByTitle(string tableName, string columnName, string text)
        {
            using (SqlConnection con = new SqlConnection(context.Database))
            {
                try
                {
                    object value;
                    SqlCommand cmd = null;
                    if (!string.IsNullOrEmpty(columnName))
                        cmd = new SqlCommand($"select top 1 {columnName} from {tableName} (nolock) where {DatabaseObjects.Columns.Title} = '{text}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and closed=0", con);
                    else
                        cmd = new SqlCommand($"select top 1 * from {tableName} (nolock) where {DatabaseObjects.Columns.Title} = '{text}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and closed=0", con);

                    con.Open();
                    value = cmd.ExecuteScalar();
                    con.Close();
                    return value;
                }catch(Exception ex)
                {
                    ULog.WriteException(ex);
                    return null;
                }
            }
        }
        public int GetTitleRelatedTicketCount(string tableName, string title, string requestType = "")
        {
            using (SqlConnection con = new SqlConnection(context.Database))
            {
                try
                {
                    int count;
                    SqlCommand cmd = null;
                    if (string.IsNullOrEmpty(requestType))
                        cmd = new SqlCommand($"select count(*) from {tableName} where ({DatabaseObjects.Columns.TicketClosed} <> 1 or Closed is null) and ({DatabaseObjects.Columns.Deleted}<>1 or Deleted is null) and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.TenantID} is not null and FREETEXT(Title,'{title}')", con);
                    else
                        cmd = new SqlCommand($"select count(*) from {tableName} where ({DatabaseObjects.Columns.TicketClosed} <> 1 or Closed is null) and ({DatabaseObjects.Columns.Deleted}<>1 or Deleted is null)and {DatabaseObjects.Columns.TicketRequestTypeLookup}={requestType}  and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.TenantID} is not null and FREETEXT(Title,'{title.Trim()}')", con);

                    con.Open();
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    return count;
                }catch(Exception ex)
                {
                    ULog.WriteException (ex);
                    return -1;
                }
            }
        }

        public DataRow GetByID(UGITModule module, long id, List<string> viewFields = null)
        {
            DataRow dtResult = null;
            string fields = "*";
            if (viewFields != null && viewFields.Count > 0)
                fields = string.Join(",", viewFields);
            using (SqlConnection con = new SqlConnection(context.Database))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(string.Format("Select TOP 1 {5} from {0} where {1}={2} and {3}='{4}' and {3} is not null", module.ModuleTable, DatabaseObjects.Columns.ID, id, DatabaseObjects.Columns.TenantID, context.TenantID, fields), con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    adp.Fill(data);
                    if (data != null && data.Rows.Count > 0)
                    {
                        dtResult = data.Rows[0];
                        return dtResult;
                    }
                    else
                    {
                        return null;
                    }
                }catch(Exception ex)
                {
                    ULog.WriteException(ex);
                    return null;
                }
            }
        }

        public int Save(UGITModule module, DataRow row)
        {
            try
            {
                int rowEffected = SaveTnx(module.ModuleTable, row);
                //ID = Convert.ToString(row[DatabaseObjects.Columns.ID]);
                if (rowEffected > 0)
                {
                    UpdateTicketCache(row, module, false);
                    return 1;
                }
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            return 0;
        }

        public int Save(UGITModule module, DataRow row, out string ID)
        {
            ID = "";
            try
            {
                int rowEffected = SaveTnx(module.ModuleTable, row);
                ID = Convert.ToString(row[DatabaseObjects.Columns.ID]);
                if (rowEffected > 0)
                {
                    UpdateTicketCache(row, module, false);
                    return 1;
                }
            }catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return 0;
        }

        public int Delete(UGITModule module, DataRow row)
        {
            try
            {
                UpdateTicketCache(row, module, true);
                int rowEffected = DeleteTnx(module.ModuleTable, row); ;
                if (rowEffected > 0)
                {
                    return 1;
                }
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            return 0;
        }

        public int SaveArchive(UGITModule module, DataRow row)
        {
            try
            {
                int rowEffected = SaveTnx(string.Format("{0}_Archive", module.ModuleTable), row);
                if (rowEffected > 0)
                {
                    return 1;
                }
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            return 0;
        }

        private int SaveTnx(string tableName, DataRow row)
        {
            if (row.RowState == DataRowState.Unchanged || row.RowState == DataRowState.Detached)
            {
                return -1;
            }

            using (SqlConnection con = new SqlConnection(context.Database))
            {
                try
                {
                    int effectedRows = 0;

                    SqlDataAdapter adp = new SqlDataAdapter(string.Format("select * from {0} (nolock) where {1} = '{2}' and {1} is not null", tableName, DatabaseObjects.Columns.TenantID, context.TenantID), con);
                    adp.UpdateBatchSize = 1;
                    SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                    builder.ConflictOption = ConflictOption.OverwriteChanges;
                    adp.RowUpdated += Adp_RowUpdated;

                    if (row.RowState == DataRowState.Added)
                    {
                        if (UGITUtility.StringToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsMigrateddata"]))
                        {
                            if (context.CurrentUser != null)
                            {
                                if (row[DatabaseObjects.Columns.ModifiedByUser] == null)
                                    row[DatabaseObjects.Columns.ModifiedByUser] = context.CurrentUser.Id;
                                if (row[DatabaseObjects.Columns.CreatedByUser] == null)
                                    row[DatabaseObjects.Columns.CreatedByUser] = context.CurrentUser.Id;
                                row[DatabaseObjects.Columns.TenantID] = context.CurrentUser.TenantID;
                            }
                        }
                        else
                        {
                            row[DatabaseObjects.Columns.Created] = DateTime.Now;
                            row[DatabaseObjects.Columns.Modified] = DateTime.Now;
                            row[DatabaseObjects.Columns.CreatedByUser] = context.CurrentUser.Id;
                            row[DatabaseObjects.Columns.ModifiedByUser] = context.CurrentUser.Id;
                            row[DatabaseObjects.Columns.TenantID] = context.TenantID;
                        }

                        // Check if column exists in table
                        //if(UGITUtility.IsSPItemExist(row, DatabaseObjects.Columns.Deleted))
                        if (row.Table.Columns.Contains(DatabaseObjects.Columns.Deleted))
                            row[DatabaseObjects.Columns.Deleted] = false;

                        // Check if column exists in table
                        //if (row.Table.Columns.Contains(DatabaseObjects.Columns.IsDeletedColumn))

                        //    row[DatabaseObjects.Columns.IsDeletedColumn] = false;

                        //SLADisabled is set to false to fix issue relate to PRS module
                        if (!UGITUtility.StringToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsMigrateddata"]))
                        {
                            if (row.Table.Columns.Contains(DatabaseObjects.Columns.SLADisabled))
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
                        effectedRows = adp.Update(insertRows);
                    }
                    else if (row.RowState == DataRowState.Modified)
                    {
                        row[DatabaseObjects.Columns.Modified] = DateTime.Now;
                        if (context.CurrentUser != null)
                        {
                            row[DatabaseObjects.Columns.ModifiedByUser] = context.CurrentUser.Id;
                            row[DatabaseObjects.Columns.TenantID] = context.CurrentUser.TenantID;
                        }

                        DataRow[] updatedRows = new DataRow[] { row };
                        adp.UpdateCommand = builder.GetUpdateCommand(true);
                        effectedRows = adp.Update(updatedRows);
                    }
                    if (effectedRows > 0)
                    {
                        row.AcceptChanges();

                        return 1;
                    }
                }catch(Exception ex)
                {
                    ULog.WriteException(ex);
                }
                return 0;
            }
        }

        private int DeleteTnx(string tableName, DataRow row)
        {
            using (SqlConnection con = new SqlConnection(context.Database))
            {
                try
                {
                    row.Delete();
                    int effectedRows = 0;
                    SqlDataAdapter adp = new SqlDataAdapter(string.Format("select * from {0} (nolock)", tableName), con);
                    SqlCommandBuilder sqlCommand = new SqlCommandBuilder(adp);

                    adp.UpdateBatchSize = 1;
                    SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                    builder.ConflictOption = ConflictOption.OverwriteChanges;
                    adp.RowUpdated += Adp_RowUpdated;
                    DataRow[] insertRows = new DataRow[] { row };
                    SqlCommand deleteCmd = builder.GetDeleteCommand(true).Clone();
                    adp.DeleteCommand = deleteCmd;
                    builder.Dispose();
                    effectedRows = adp.Update(insertRows);

                    if (effectedRows > 0)
                    {
                        return 1;
                    }
                }catch(Exception ex)
                {
                    ULog.WriteException(ex);
                }
                return 0;
            }
        }

        private void Adp_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            try
            {
                if (e.StatementType == StatementType.Insert)
                {
                    object id = e.Command.Parameters[string.Format("@{0}", DatabaseObjects.Columns.ID)].Value;
                    e.Row[DatabaseObjects.Columns.ID] = id;
                    e.Row.AcceptChanges();
                }
            }catch(Exception ex) { ULog.WriteException(ex); }
        }

        public DataTable GetTickets(string query)
        {
            var dtResult = new DataTable();
            using (var con = new SqlConnection(context.Database))
            {
                try
                {
                    var cmd = new SqlCommand(query, con);
                    var adp = new SqlDataAdapter(cmd);

                    adp.Fill(dtResult);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
                return dtResult;
            }
        }

        public void UpdateTicketCache(DataRow currentTicket, UGITModule moduleName, bool Archive = false)
        {
            try
            {
                if (currentTicket == null)
                    return;
                string keyClose = string.Format("ClosedTicket_{0}", moduleName.ModuleName);
                string keyOpen = string.Format("OpenTicket_{0}", moduleName.ModuleName);

                bool openStatus = CacheHelper<object>.IsExists(keyOpen, context.TenantID);
                bool closeStatus = CacheHelper<object>.IsExists(keyClose, context.TenantID);
                DataTable dtClose = ((DataTable)CacheHelper<object>.Get(keyClose, context.TenantID));
                DataTable dtOpen = ((DataTable)CacheHelper<object>.Get(keyOpen, context.TenantID));

                if (!openStatus && (dtOpen == null || dtOpen.Rows.Count == 0))
                {

                    dtOpen = GetOpenTickets(moduleName);
                    CacheHelper<object>.AddOrUpdate(keyOpen, context.TenantID, dtOpen);
                }
                if (!closeStatus && (dtClose == null || dtClose.Rows.Count == 0))
                {
                    dtClose = GetClosedTickets(moduleName);
                    CacheHelper<object>.AddOrUpdate(keyClose, context.TenantID, dtClose);
                }

                DataRow[] drClose = null;
                DataRow[] drOpen = null;
                if (dtClose != null && dtClose.Rows.Count > 0 && dtClose.Columns.Count > 0)
                    drClose = dtClose.Select(DatabaseObjects.Columns.TicketId + "='" + Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]) + "'");
                if (dtOpen != null && dtOpen.Rows.Count > 0 && dtOpen.Columns.Count > 0)
                    drOpen = dtOpen.Select(DatabaseObjects.Columns.TicketId + "='" + Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]) + "'");
                if (drOpen != null && drOpen.Count() > 0 && Archive)
                {
                    foreach (DataRow dr in drOpen) { dtOpen.Rows.Remove(dr); }
                    return;
                }
                if (drClose != null && drClose.Count() > 0 && Archive)
                {
                    foreach (DataRow dr in drClose) { dtClose.Rows.Remove(dr); };
                    return;
                }

                if (Convert.ToString(currentTicket[DatabaseObjects.Columns.Closed]) == "True")
                {
                    DataRow drItem = GetCurrentTicketbyId(currentTicket, moduleName);
                    if (drOpen != null && drOpen.Count() > 0)
                    {
                        dtOpen.Rows.Remove(drOpen[0]);
                        if (dtClose != null)
                        {
                            if (closeStatus && dtClose.Rows.Count > 0)
                            {
                                dtClose.Rows.Add(drItem.ItemArray); //dtClose.Rows.Add(currentTicket.ItemArray);
                                CacheHelper<object>.AddOrUpdate(keyClose, dtClose);
                            }
                            else
                            {
                                DataTable dtt = new DataTable();
                                dtt.ImportRow(drItem); //dtt.ImportRow(currentTicket);
                                CacheHelper<object>.AddOrUpdate(keyClose, context.TenantID, dtt);
                            }
                        }
                        else
                        {
                            if (!closeStatus)
                            {
                                DataTable dtt = new DataTable();
                                dtt.ImportRow(drItem); //dtt.ImportRow(currentTicket);
                                CacheHelper<object>.AddOrUpdate(keyClose, context.TenantID, dtt);
                            }

                        }
                    }
                    else
                    {
                        if (drClose != null && drClose.Count() > 0)
                            drClose[0].ItemArray = drItem.ItemArray; //currentTicket.ItemArray;
                        else if (dtClose.Columns.Count > 0)
                            dtClose.Rows.Add(drItem.ItemArray); //dtClose.Rows.Add(currentTicket.ItemArray);
                    }
                }
                else
                {
                    if (drClose != null && drClose.Count() > 0)
                    {
                        DataRow drItem = GetCurrentTicketbyId(currentTicket, moduleName);
                        foreach (DataRow dr in drClose)
                        {
                            dtClose.Rows.Remove(dr);
                        }
                        if (dtOpen != null)
                        {
                            if (openStatus)
                                dtOpen.Rows.Add(drItem.ItemArray); //dtOpen.Rows.Add(currentTicket.ItemArray);
                            else
                            {
                                DataTable dtt = new DataTable();
                                dtt.ImportRow(drItem); //dtt.ImportRow(currentTicket);
                                CacheHelper<object>.AddOrUpdate(keyOpen, context.TenantID, dtt);
                            }

                        }
                        else
                        {
                            if (!openStatus)
                            {
                                DataTable dtt = new DataTable();
                                dtt.ImportRow(drItem); //dtt.ImportRow(currentTicket);
                                CacheHelper<object>.AddOrUpdate(keyOpen, context.TenantID, dtt);
                            }

                        }
                    }
                    else
                    {
                        DataRow drItem = GetCurrentTicketbyId(currentTicket, moduleName);
                        if (drOpen != null && drOpen.Count() > 0 && drOpen[0] != null)
                        {
                            // added below code lines to update current ticket row in open tickets table
                            int rowIndex = dtOpen.Rows.IndexOf(drOpen[0]);
                            if (rowIndex >= 0)
                                dtOpen.Rows[rowIndex].ItemArray = drItem.ItemArray; //currentTicket.ItemArray;
                        }
                        else
                        {
                            if (dtOpen != null && dtOpen.Rows.Count > 0)
                                dtOpen.Rows.Add(drItem.ItemArray); //dtOpen.Rows.Add(currentTicket.ItemArray);
                            else
                                dtOpen.Rows.Add(drItem.ItemArray);//dtOpen = currentTicket.Table;
                        }

                        // Update open ticket cache
                        if (dtOpen != null)
                        {
                            dtOpen.AcceptChanges();
                            CacheHelper<object>.AddOrUpdate(keyOpen, context.TenantID, dtOpen);
                        }
                    }
                }

                DataTable dtAll = new DataTable();
                if (dtOpen != null && dtOpen.Rows.Count > 0)
                    dtAll.Merge(dtOpen);

                if (dtClose != null && dtClose.Rows.Count > 0)
                    dtAll.Merge(dtClose);

                if (dtAll != null && dtAll.Rows.Count > 0)
                    CacheHelper<object>.AddOrUpdate($"AllTicket_{moduleName.ModuleName}", context.TenantID, dtAll);
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        public DataTable GetCachedModuleTableSchema(UGITModule module)
        {
            try
            {
                DataTable dtResult = new DataTable();
                string query = string.Format("select Top(0) * from {0} WITH(NOLOCK)", module.ModuleTable);
                string keyOpen = string.Format("OpenTicket_{0}", module.ModuleName);
                var moduleTable = (DataTable)CacheHelper<object>.Get(keyOpen, context.TenantID);
                if (moduleTable == null)
                    dtResult = GetTickets(query);
                else
                    dtResult = moduleTable.Clone();
                return dtResult;
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }

        }
        public DataTable GetDatabaseTableSchema(string tablename)
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection con = new SqlConnection(context.Database))
            {
                try
                {
                    string query = string.Format("Select Top(0) * from {0} ", tablename);
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtResult);
                    return dtResult;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    return dtResult;
                }
            }
        }
        public DataTable GetTickets(string table, string where)
        {
            var dtResult = new DataTable();

            using (var con = new SqlConnection(context.Database))
            {
                try
                {
                    string query = string.Format("select * from {0} ", table);

                    string tenantQ = $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'";
                    if (string.IsNullOrWhiteSpace(where))
                        where = tenantQ;
                    else if (!where.Contains(DatabaseObjects.Columns.TenantID))
                    {
                        where = $"{tenantQ} and ({where})";
                    }

                    if (!string.IsNullOrWhiteSpace(where))
                    {
                        where = "where " + where;
                        query += where;
                    }
                    var cmd = new SqlCommand(query, con);
                    var adp = new SqlDataAdapter(cmd);

                    adp.Fill(dtResult);
                    return dtResult;
                }catch(Exception ex)
                {
                    ULog.WriteException(ex);
                    return dtResult;
                }
            }
        }

        public DataTable GetAllTicketsWithOutCache(UGITModule module)
        {
            try
            {
                DataTable dataResult;
                string query = $"select * from {module.ModuleTable} (nolock) where  ({DatabaseObjects.Columns.Deleted}<>1 or Deleted is null) and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.TenantID} is not null";
                dataResult = GetTickets(query);
                return dataResult;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
        }

        public static DataRow GetCurrentTicketbyId(DataRow currentTicket, UGITModule moduleName)
        {
            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", Convert.ToString(currentTicket[DatabaseObjects.Columns.TenantID]));
                values.Add("@IsClosed", "");
                values.Add("@TicketId", Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]));
                DataTable collection = uGITDAL.ExecuteDataSetWithParameters($"usp_Get{moduleName.ModuleTable}", values);

                if (collection != null && collection.Rows.Count > 0)
                    return collection.Select()[0];
                else
                    return currentTicket;
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
        }

        public void UpdateTicketCache(UGITModule moduleName)
        {
            try
            {
                string keyClose = string.Format("ClosedTicket_{0}", moduleName.ModuleName);
                string keyOpen = string.Format("OpenTicket_{0}", moduleName.ModuleName);
                bool openStatus = CacheHelper<object>.IsExists(keyOpen, context.TenantID);
                bool closeStatus = CacheHelper<object>.IsExists(keyClose, context.TenantID);
                if (openStatus)
                    openStatus = false;
                if (closeStatus)
                    closeStatus = false;
                DataTable dtClose = null;
                DataTable dtOpen = null;
                CacheHelper<object>.Delete(keyClose, context.TenantID);
                CacheHelper<object>.Delete(keyOpen, context.TenantID);
                if (!openStatus && (dtOpen == null || dtOpen.Rows.Count == 0))
                {
                    dtOpen = GetOpenTickets(moduleName);
                    CacheHelper<object>.AddOrUpdate(keyOpen, context.TenantID, dtOpen);
                }
                if (!closeStatus && (dtClose == null || dtClose.Rows.Count == 0))
                {
                    dtClose = GetClosedTickets(moduleName);
                    CacheHelper<object>.AddOrUpdate(keyClose, context.TenantID, dtClose);
                }
                DataTable dtAll = new DataTable();
                if (dtOpen != null && dtOpen.Rows.Count > 0)
                    dtAll.Merge(dtOpen);

                if (dtClose != null && dtClose.Rows.Count > 0)
                    dtAll.Merge(dtClose);

                if (dtAll != null && dtAll.Rows.Count > 0)
                    CacheHelper<object>.AddOrUpdate($"AllTicket_{moduleName.ModuleName}", context.TenantID, dtAll);
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
        }
    }
}


