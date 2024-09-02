using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager
{
    public class WikiReviewsManager : ManagerBase<WikiReviews>, IWikiReviewsManager
    {
        public WikiReviewsManager(ApplicationContext context) :base(context)
        {

        }
    }

    public interface IWikiReviewsManager : IManagerBase<WikiReviews>
    {

    }

}
