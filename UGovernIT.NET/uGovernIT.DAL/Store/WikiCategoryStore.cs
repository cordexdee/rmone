using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
   public  class WikiCategoryStore : StoreBase<WikiCategory>, IWikiCategoryStore
    {
        public WikiCategoryStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IWikiCategoryStore : IStore<WikiCategory>
    {

    }
}
