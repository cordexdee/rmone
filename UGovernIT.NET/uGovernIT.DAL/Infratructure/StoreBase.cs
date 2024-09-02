using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.DAL.Infratructure;
using Microsoft.EntityFrameworkCore;
using uGovernIT.Utility;
using uGovernIT.Util.Log;
using System.Diagnostics;

namespace uGovernIT.DAL
{
    public class StoreBase<T> : IStore<T> where T : class
    {
        protected string tableName;
        protected CustomDbContext context;

        public StoreBase(CustomDbContext context, string tableName = "")
        {
            this.context = context;
            this.tableName = tableName;

            TableAttribute tAttr = Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute)) as TableAttribute;
            if (tAttr != null)
            {
                this.tableName = tAttr.Name;
            }
        }

        public virtual List<T> Load(string where)
        {
            if (!string.IsNullOrWhiteSpace(where) && !where.ToLower().Trim().StartsWith("where"))
            {
                where = "where " + where;
            }

            List<T> items;

            using (var ctx = new DatabaseContext(context))
            {
                try
                {
                    var query = $"Select * from {tableName} (nolock) {where}";
                    items = ctx.Set<T>().FromSql(query).ToList();
                }catch(Exception ex)
                {
                    items = null;
                    ULog.WriteException(ex);
                }
            }
            return items;
        }

        public virtual List<T> Load()
        {
            List<T> dataLst;

            using (var ctx = new DatabaseContext(context))
            {
                try
                {
                    dataLst = ctx.Set<T>().ToList();
                }
                catch (Exception ex)
                {
                    dataLst = null;
                    ULog.WriteException(ex);
                }
            }
            return dataLst;
        }

        public virtual T LoadByID(long ID)
        {
            T item = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    item = ctx.Set<T>().Find(ID);
                }
                catch (Exception ex)
                {
                    item = null;
                    ULog.WriteException(ex);
                }

            }
            return item;
        }

        public int Count()
        {
            int recordCount = 0;

            using (var ctx = new DatabaseContext(context))
            {
                try
                {
                    recordCount = ctx.Set<T>().Count();
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            return recordCount;
        }

        public long Insert(T obj)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    ctx.Set<T>().Add(obj);
                    ctx.SaveChanges();
                    return 1;
                }
                catch (Exception ex)
                {
                    StackTrace objstackTrace = new StackTrace();
                    StackFrame objStackFrame = objstackTrace.GetFrame(1);
                    string callingMethod = objStackFrame.GetMethod().Name;
                    string callingClass = objStackFrame.GetMethod().DeclaringType.FullName;

                    Type objType = typeof(T);
                    ULog.WriteException($"Calling Method: {callingMethod}: Calling Class: {callingClass} : An Exception Occurred when Inserting type of {objType.Name}: {ex}");
                    return 0;
                }
            }
        }

        public bool TenantUpdate(T obj)
        {


            using (CommonDatabaseContext ctx = new CommonDatabaseContext(context))
            {
                try
                {
                    ctx.Set<T>().Update(obj);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    return false;
                }
            }
        }

        public bool Update(T obj)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    ctx.Set<T>().Update(obj);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    StackTrace objstackTrace = new StackTrace();
                    StackFrame objStackFrame = objstackTrace.GetFrame(1);
                    string callingMethod = objStackFrame.GetMethod().Name;
                    string callingClass = objStackFrame.GetMethod().DeclaringType.FullName;

                    Type objType = typeof(T);
                    ULog.WriteException($"Calling Method: {callingMethod}: Calling Class: {callingClass} : An Exception Occurred when Updating type of {objType.Name}: {ex}");
                    return false;
                }
            }
        }

        public bool Delete(List<T> obj)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    ctx.Set<T>().RemoveRange(obj);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    StackTrace objstackTrace = new StackTrace();
                    StackFrame objStackFrame = objstackTrace.GetFrame(1);
                    string callingMethod = objStackFrame.GetMethod().Name;
                    string callingClass = objStackFrame.GetMethod().DeclaringType.FullName;

                    Type objType = typeof(T);
                    ULog.WriteException($"Calling Method: {callingMethod}: Calling Class: {callingClass} : An Exception Occurred when deleting type of {objType.Name}: {ex}");
                    return false;
                }
            }
        }

        public bool Delete(T obj)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    ctx.Set<T>().Remove(obj);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex) {
                    StackTrace objstackTrace = new StackTrace();
                    StackFrame objStackFrame = objstackTrace.GetFrame(1);
                    string callingMethod = objStackFrame.GetMethod().Name;
                    string callingClass = objStackFrame.GetMethod().DeclaringType.FullName;

                    Type objType = typeof(T);
                    ULog.WriteException($"Calling Method: {callingMethod}: Calling Class: {callingClass} : An Exception Occurred when deleting type of {objType.Name}: {ex}");
                    return false;
                }
            }
        }

        public bool Recycle(List<T> obj)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    // Set delete for recycle object
                    DBBaseEntity baseEntity = null;
                    obj.ForEach(x =>
                    {
                        baseEntity = x as DBBaseEntity;
                        if (baseEntity != null)
                            baseEntity.Deleted = true;
                    });
                    ctx.Set<T>().UpdateRange(obj);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    return false;
                }
            }
        }

        public bool Recycle(T obj)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    DBBaseEntity baseEntity = obj as DBBaseEntity;
                    if (baseEntity != null)
                    {
                        baseEntity.Deleted = true;
                    }
                    ctx.Set<T>().Update(obj);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    return false;
                }
            }
        }

        public bool Restore(List<T> obj)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    // Set delete for recycle object
                    DBBaseEntity baseEntity = null;
                    obj.ForEach(x =>
                    {
                        baseEntity = x as DBBaseEntity;
                        if (baseEntity != null)
                            baseEntity.Deleted = false;
                    });
                    ctx.Set<T>().UpdateRange(obj);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    return false;
                }
            }
        }

        public bool Restore(T obj)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    DBBaseEntity baseEntity = obj as DBBaseEntity;
                    if (baseEntity != null)
                    {
                        baseEntity.Deleted = false;
                    }
                    ctx.Set<T>().Update(obj);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    return false;
                }
            }
        }

        public virtual T Get(Expression<Func<T, bool>> where, Expression<Func<T, T>> order = null, List<Expression<Func<T, object>>> includeExpressions = null)
        {
            T item = null;

            DbContextOptionsBuilder ss = new DbContextOptionsBuilder();
            ss.UseSqlServer(context.Database);

            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    var query = ctx.Set<T>().Where(where);
                    if (order != null)
                        query = query.OrderBy(order);

                    if (includeExpressions != null)
                    {
                        foreach (var includeExpression in includeExpressions)
                        {
                            query = query.Include(includeExpression);
                        }
                    }
                    item = query.Take(1).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    item = null;
                    ULog.WriteException(ex);
                }
                
            }

            return item;
        }

        private IEnumerable<T> GetMany<TOrder>(Expression<Func<T, bool>> where, Expression<Func<T, object>> order = null, int skip = 0, int take = 0, List<Expression<Func<T, object>>> includeExpressions = null)
        {
            List<T> dataLst = new List<T>();
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    var dataQ = ctx.Set<T>().Where(where);
                    if (includeExpressions != null)
                    {
                        foreach (var includeExpression in includeExpressions)
                        {
                            dataQ = dataQ.Include(includeExpression);
                        }
                    }
                    if (order != null)
                        dataQ = dataQ.OrderBy(order);

                    dataQ = dataQ.Skip(skip);

                    if (take > 0)
                        dataQ = dataQ.Take(take);

                    dataLst = dataQ.ToList();
                }
                catch(Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            return dataLst;
        }

        public virtual void UpdateItems(List<T> obj)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    ctx.UpdateRange(obj);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
        }

        public virtual bool InsertItems(List<T> obj)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    ctx.AddRange(obj);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    return false;
                }
            }
        }

        public virtual List<T> Load(Expression<Func<T, bool>> where, Expression<Func<T, object>> order = null, int skip = 0, int take = 0, List<Expression<Func<T, object>>> includeExpressions = null)
        {
            return GetMany<T>(where, order, skip, take, includeExpressions).ToList();
        }

        public virtual T Get(object ID)
        {
            T item = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    item = ctx.Find<T>(ID);
                }
                catch (Exception ex)
                {
                    item = null;
                    ULog.WriteException(ex);
                }
            }
            return item;

        }

        public virtual T Get(string where)
        {
            T item = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    string query = string.Format("Select Top(1) * from {0}  {1}", tableName, where);
                    item = ctx.Set<T>().FromSql(query).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    item = null;
                    ULog.WriteException(ex);
                }
                
            }
            return item;
        }

        public virtual DataTable GetDataTable(string where)
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection con = new SqlConnection(context.Database))
            {
                try
                {
                    string query = "Select * from " + tableName + " (nolock) " + where;
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

        public virtual DataTable GetSchema()
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection con = new SqlConnection(context.Database))
            {
                try
                {
                    string query = string.Format("Select Top(0) * from {0} where TABLE_NAME='{1}'", DatabaseObjects.Tables.InformationSchema, tableName);
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

        public virtual int ExecuteStoredProcedure(string tenantId, string accountId)
        {
            int affectedRows = 0;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                    affectedRows = ctx.Database.ExecuteSqlCommand("populate_default_tenant_data @TenantId", new SqlParameter("@TenantId", tenantId));
                    affectedRows += ctx.Database.ExecuteSqlCommand("Update_default_tenant_KeyValue @TenantId, @AccountId", new SqlParameter("@TenantId", tenantId), new SqlParameter("@AccountId", accountId));
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
                
            }
            return affectedRows;
        }

        public virtual Tenant GetTenant(string accountId)
        {
            Tenant tenant = null;

            using (CommonDatabaseContext ctx = new CommonDatabaseContext(context))
            {
                try
                {
                    tenant = ctx.Tenants.FirstOrDefault(x => x.AccountID.Equals(accountId, StringComparison.InvariantCultureIgnoreCase));
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            return tenant;
        }

        public virtual Tenant GetTenantById(string tenantId)
        {
            Guid guidtenantId = Guid.Parse(tenantId);
            Tenant tenant = null;

            using (CommonDatabaseContext ctx = new CommonDatabaseContext(context))
            {
                try
                {
                    tenant = ctx.Tenants.FirstOrDefault(x => x.TenantID == guidtenantId);//.Equals(guidtenantId, StringComparison.InvariantCultureIgnoreCase));
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            return tenant;
        }

        public virtual bool CreateDatabaseBackup(string path, string accountId)
        {
            bool success = false;

            try
            {
                using (DatabaseContext ctx = new DatabaseContext(context))
                {
                    // command timeout to 180 seconds
                    ctx.Database.SetCommandTimeout(180);

                    ctx.Database.ExecuteSqlCommand($"CreateDatabaseBackUp @Path, @AccountId", new SqlParameter("@Path", path), new SqlParameter("@AccountId", accountId));
                }

                success = true;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                throw ex;
            }

            return success;
        }

        public virtual bool DeleteTenantData(string tenantId)
        {
            bool success = false;

            try
            {
                using (DatabaseContext ctx = new DatabaseContext(context))
                {
                    // command timeout to 180 seconds
                    ctx.Database.SetCommandTimeout(180);

                    ctx.Database.ExecuteSqlCommand($"DeleteTenantData @TenantId", new SqlParameter("@TenantId", tenantId));
                }

                success = true;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                throw ex;
            }

            return success;
        }

        public virtual bool DeleteTenant(string tenantId)
        {
            bool success = false;

            try
            {
                using (CommonDatabaseContext ctx = new CommonDatabaseContext(context))
                {
                    ctx.Database.ExecuteSqlCommand($"DELETE FROM Tenant WHERE TenantID = @TenantId", new SqlParameter("@TenantId", tenantId));
                }

                success = true;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                throw ex;
            }

            return success;
        }
    }
}
