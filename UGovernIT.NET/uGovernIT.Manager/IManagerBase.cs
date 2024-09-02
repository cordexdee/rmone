using DevExpress.DataAccess.Native.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager
{
    public interface IManagerBase<T> 
    {
        long Insert(T item);
        bool Update(T item);
        bool Delete(T item);
        List<T> Load(string where);
        List<T> Load(Expression<Func<T, bool>> where, Expression<Func<T, object>> order = null, int skip = 0, int take = 0, List<Expression<Func<T, object>>> includeExpressions = null);
        List<T> Load();
        T LoadByID(long ID);

        T Get(object ID);
        T Get(string where);
        T Get(Expression<Func<T, bool>> where, Expression<Func<T, T>> order = null, List<Expression<Func<T, object>>> includeExpressions = null);
        void UpdateItems(List<T> itemList);



        System.Data.DataTable GetDataTable(string where = "");
    }
}
