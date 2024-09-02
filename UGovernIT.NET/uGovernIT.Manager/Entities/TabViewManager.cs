using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Cache;

namespace uGovernIT.Manager
{
    public class TabViewManager:ManagerBase<TabView>, ITabViewManager
    {
        public TabViewManager(ApplicationContext context):base(context)
        {
            store = new TabViewStore(this.dbContext);

        }
        public List<TabView> GetTabsByViewName(string viewName)
        {
            List<TabView> aResult = new List<TabView>();
            aResult = store.Load(x => x.ViewName == viewName && x.ShowTab == true); // string.Format(" where {0} = '{1}'", DatabaseObjects.Columns.ViewName, viewName));
            aResult = aResult.OrderBy(x => x.TabOrder).ToList();
            return aResult;
        }
        public bool Remove(List<TabView> items)
        {
            return this.Delete(items);
        }

    }
    public interface ITabViewManager : IManagerBase<TabView>
    {
        List<TabView> GetTabsByViewName(string viewName);
    }
}
