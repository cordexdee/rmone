using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class SubContractorManager : ManagerBase<SubContractor>, ISubContractor
    {

        public SubContractorManager(ApplicationContext context) : base(context)
        {
            store = new SubContractorStore(this.dbContext);
        }
    }

    public interface ISubContractor : IManagerBase<SubContractor>
    {
    }
}
