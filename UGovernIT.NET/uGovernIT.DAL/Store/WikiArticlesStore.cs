using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL.Store
{
    public class WikiArticlesStore : StoreBase<WikiArticles>, IWikiArticlesStore
    {
        public WikiArticlesStore(CustomDbContext context) :base(context)
        {

        }
    }

    public interface IWikiArticlesStore: IStore<WikiArticles>
    {

    }
}
