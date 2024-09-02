using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public interface IPMMCommentHistoryManager : IManagerBase<PMMCommentHistory>
    {

    }

   public class PMMCommentHistoryManager:ManagerBase<PMMCommentHistory>,IPMMCommentHistoryManager
    {
        public PMMCommentHistoryManager(ApplicationContext context):base(context)
        {
            store = new PMMCommentHistoryStore(this.dbContext);
        }
    }
}
