using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public interface IRelatedCompanyStore : IStore<RelatedCompany>
    {
    }

    public class RelatedCompanyStore : StoreBase<RelatedCompany>, IRelatedCompanyStore
    {
        public RelatedCompanyStore(CustomDbContext context) : base(context)
        {
        }
    }    
}
