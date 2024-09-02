using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL.Store
{
    public class WikiMenuLeftNavigationStore : StoreBase<WikiCategory>, IWikiMenuLeftNavigationStore
    {
        public WikiMenuLeftNavigationStore(CustomDbContext context) : base(context)
        {

        }


    }


    public interface IWikiMenuLeftNavigationStore : IStore<WikiCategory>
    {

    }

}
