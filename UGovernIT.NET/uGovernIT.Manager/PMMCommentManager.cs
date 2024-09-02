using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class PMMCommentManager : ManagerBase<PMMComments>, IPMMCommentManager
    {
        public PMMCommentManager(ApplicationContext context) : base(context)
        {
            store = new PMMCommentStore(this.dbContext);
        }
    }
    public interface IPMMCommentManager : IManagerBase<PMMComments>
    {

    }
}
