using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
   public class WikiCategoryManager : ManagerBase<WikiCategory>, IWikiCategoryManager
    {
        public WikiCategoryManager(ApplicationContext context):base(context)
        {
            store = new WikiCategoryStore(this.dbContext);
        }

        public WikiCategory AddOrUpdate(WikiCategory wikiCategory)
        {
            if (wikiCategory != null)
            {
                if (wikiCategory.ID > 0)
                    this.Update(wikiCategory);
                else
                    this.Insert(wikiCategory);
            }
            return wikiCategory;
        }

    }
    public interface IWikiCategoryManager : IManagerBase<WikiCategory>
    {
        WikiCategory AddOrUpdate(WikiCategory wikiCategory);

    }
}
