using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities.DB;


namespace uGovernIT.Manager.Managers
{
    public class AgentsManager: ManagerBase<Agents>
    {
        public AgentsManager(ApplicationContext context) : base(context)
        {
            store = new AgentsStore(this.dbContext);
        }
    }
}
