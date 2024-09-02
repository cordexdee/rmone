using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public interface IPMMCommentHistoryStore: IStore<PMMCommentHistory>
    {


    }

    public class PMMCommentHistoryStore: StoreBase<PMMCommentHistory>,IPMMCommentHistoryStore
    {
        public PMMCommentHistoryStore(CustomDbContext context) :base(context)
        {

        }

    }
}
