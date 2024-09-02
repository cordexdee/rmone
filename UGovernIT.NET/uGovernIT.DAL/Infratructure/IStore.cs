using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL
{
    public interface IStore<T> where T: class
    {
        List<T> Load(string where);
        List<T> Load(Expression<Func<T, bool>> where, Expression<Func<T, object>> order = null, int skip=0, int take=0, List<Expression<Func<T, object>>> includeExpressions = null);
        List<T> Load();

        T LoadByID(long ID);
        int Count();
        long Insert(T item);
        bool Update(T item);
        bool TenantUpdate(T item);
        bool Delete(T item);
        bool Delete(List<T> items);

        bool Recycle(T item);
        bool Recycle(List<T> items);

        bool Restore(T item);
        bool Restore(List<T> items);

        T Get(object ID);
        T Get(string where);
        T Get(Expression<Func<T, bool>> where, Expression<Func<T, T>> order = null, List<Expression<Func<T, object>>> includeExpressions = null);

        void UpdateItems(List<T> obj);
        bool InsertItems(List<T> obj);

        Tenant GetTenant(string accountId);
        Tenant GetTenantById(string tenantId);

        int ExecuteStoredProcedure(string tenantId, string accountId);

        bool CreateDatabaseBackup(string path, string accountId);
        bool DeleteTenantData(string tenantId);
        bool DeleteTenant(string tenantId);
    }
}
