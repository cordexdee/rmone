using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager
{
    public class WikiArticlesManager : ManagerBase<WikiArticles>, IWikiArticlesManager
    {
        public WikiArticlesManager(ApplicationContext context) :base(context)
        {
            store = new WikiArticlesStore(this.dbContext);
        }
    }

   public interface IWikiArticlesManager :IManagerBase<WikiArticles>
    {

    }
}
