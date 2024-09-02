using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using uGovernIT.DAL;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ManagerBase<T> : IManagerBase<T> where T : class
    {
        protected ApplicationContext dbContext;
        protected IStore<T> store;

        //public ManagerBase()
        //{
        //    dbContext = ApplicationContext.Create();
        //    store = new StoreBase<T>(dbContext);
        //}

        //public ManagerBase(ApplicationContext context)
        //{
        //    dbContext = context;
        //    store = new StoreBase<T>(dbContext);
        //}

        public ManagerBase(ApplicationContext context, string request = "")
        {

            if (  request == "SelfRegistration")
            {
                dbContext = ApplicationContext.Create();
            }
            else
            {
                dbContext = context;
            }
            store = new StoreBase<T>(dbContext);
        }

        public virtual bool Delete(T item)
        {
            return store.Delete(item);
        }

        public virtual bool Delete(List<T> ItemList)
        {
            return store.Delete(ItemList);
        }

        public virtual long Insert(T item)
        {
            return store.Insert(item);
        }

        public virtual List<T> Load()
        {
            return store.Load();
        }

        public virtual T LoadByID(long ID)
        {
            return store.LoadByID(ID);
        }
        public virtual int Count()
        {
            return store.Count();
        }
        public virtual bool Update(T item)
        {
            return store.Update(item);
        }

        public virtual bool TenantUpdate(T item)
        {
            return store.TenantUpdate(item);
        }

        public System.Data.DataTable GetDataTable(string where = "")
        {
            return UGITUtility.ToDataTable<T>(!string.IsNullOrEmpty(where) ? store.Load(where) : store.Load());
        }

        public virtual List<T> Load(string where)
        {
            return store.Load(where);
        }

        public virtual List<T> Load(Expression<Func<T, bool>> where, Expression<Func<T, object>> order = null, int skip = 0, int take = 0, List<Expression<Func<T, object>>> includeExpressions = null)
        {
            return store.Load(where, order, skip, take, includeExpressions);
        }

        public virtual T Get(object ID)
        {
            return store.Get(ID);
        }

        public virtual T Get(string where)
        {
            return store.Get(where);
        }

        public virtual T Get(Expression<Func<T, bool>> where, Expression<Func<T, T>> order = null, List<Expression<Func<T, object>>> includeExpressions = null)
        {
            return store.Get(where, order, includeExpressions);
        }

        public virtual void UpdateItems(List<T> itemList)
        {
            store.UpdateItems(itemList);
        }

        public virtual bool InsertItems(List<T> itemList)
        {
           return store.InsertItems(itemList);
        }

        public Tenant GetTenant(string accountId)
        {
            return (store.GetTenant(accountId));
        }

        public Tenant GetTenantById(string tenantId)
        {
            return (store.GetTenantById(tenantId));
        }

        public virtual int ExecuteStoredProcedure(string tenantId, string accountId)
        {
            var row = store.ExecuteStoredProcedure(tenantId, accountId);
            return row;
        }

        public virtual bool CreateDatabaseBackup(string path, string accountId)
        {
            return store.CreateDatabaseBackup(path, accountId);
        }

        public virtual bool DeleteTenantData(string tenantId)
        {
            return store.DeleteTenantData(tenantId);
        }

        public virtual bool DeleteTenant(string tenantId)
        {
            return store.DeleteTenant(tenantId);
        }
    }
}
