using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DAL.Store;
using System.Xml;
using uGovernIT.Utility.DockPanels;
using uGovernIT.Util.Cache;

namespace uGovernIT.Manager
{
    public class PageConfigurationManager : ManagerBase<PageConfiguration>, IPageConfigurationManager
    {
        public PageConfigurationManager(ApplicationContext context): base(context)
        {
            store = new PageConfigurationStore(this.dbContext);
        }

        public PageConfiguration GetCachePage(string pageName)
        {
            PageConfiguration page = CacheHelper<PageConfiguration>.Get(pageName, this.dbContext.TenantID);
            if (page == null)
            {
                page = this.Get(x => x.Name == pageName || x.Title == pageName);
                CacheHelper<PageConfiguration>.AddOrUpdate(pageName, this.dbContext.TenantID, page);
            }
            return page;
        }

        public override bool Update(PageConfiguration item)
        {
            bool updated = base.Update(item);
            item = this.Get(x => x.ID == item.ID);
            CacheHelper<PageConfiguration>.AddOrUpdate(item.Name, dbContext.TenantID, item);
            return updated;
        }

        public override long Insert(PageConfiguration item)
        {
            long id = base.Insert(item);

            item = this.Get(x => x.ID == item.ID);
            CacheHelper<PageConfiguration>.AddOrUpdate(item.Name, dbContext.TenantID, item);
            return id;
        }

        public override bool Delete(PageConfiguration item)
        {
            CacheHelper<PageConfiguration>.Delete(item.Name, dbContext.TenantID);
            return base.Delete(item);
        }

        public void RefreshCache()
        {
            List<PageConfiguration> lstPageConfigurations = this.Load();

            if (lstPageConfigurations == null || lstPageConfigurations.Count == 0) 
                return;

            foreach (PageConfiguration pConfig in lstPageConfigurations) 
            {
                PageConfiguration page = this.Get(x => x.ID == pConfig.ID);
                CacheHelper<PageConfiguration>.AddOrUpdate(pConfig.Name, this.dbContext.TenantID, page);
            }
        }
    }
    public interface IPageConfigurationManager : IManagerBase<PageConfiguration>
    {
        PageConfiguration GetCachePage(string pageName);
    }
}
