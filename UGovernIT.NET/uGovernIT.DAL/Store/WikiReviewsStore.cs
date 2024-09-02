using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL.Store
{
    public class WikiReviewsStore : StoreBase<WikiReviews>, IWikiReviewsStore
    {
        public WikiReviewsStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IWikiReviewsStore :IStore<WikiReviews>
    {

    }
}
