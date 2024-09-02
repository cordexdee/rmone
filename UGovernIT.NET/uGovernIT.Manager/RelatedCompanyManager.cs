using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public interface IRelatedCompany : IManagerBase<RelatedCompany>
    {
    }

    public class RelatedCompanyManager : ManagerBase<RelatedCompany>, IRelatedCompany
    {
        public RelatedCompanyManager(ApplicationContext context) : base(context)
        {
            store = new RelatedCompanyStore(this.dbContext);
        }
    }
}
