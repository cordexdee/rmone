using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class CRMRelationshipTypeManager : ManagerBase<CRMRelationshipType>, ICRMRelationshipType
    {
        public CRMRelationshipTypeManager(ApplicationContext context) : base(context)
        {
            store = new CRMRelationshipTypeStore(this.dbContext);
        }
    }
    public interface ICRMRelationshipType : IManagerBase<CRMRelationshipType>
    {
    }
}