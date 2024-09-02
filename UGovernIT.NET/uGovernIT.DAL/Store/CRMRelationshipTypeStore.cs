using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class CRMRelationshipTypeStore : StoreBase<CRMRelationshipType>, ICRMRelationshipTypeStore
    {
        public CRMRelationshipTypeStore(CustomDbContext context) : base(context)
        {
        }

    }
    public interface ICRMRelationshipTypeStore : IStore<CRMRelationshipType>
    {
    }
}