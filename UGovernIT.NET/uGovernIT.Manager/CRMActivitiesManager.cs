using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
     public class CRMActivitiesManager : ManagerBase<CRMActivities>, ICRMActivities
    {

        public CRMActivitiesManager(ApplicationContext context) : base(context)
        {
            store = new CRMActivitiesStore(this.dbContext);
        }
     }
    public interface ICRMActivities : IManagerBase<CRMActivities>
    {
    }
}
