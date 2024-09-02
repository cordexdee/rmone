using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;
namespace uGovernIT.Manager
{
    public class DocumentManager : ManagerBase<Document>, IDocumentManager
    {
        public DocumentManager(ApplicationContext context) : base(context)
        {
            store = new DocumentStore(this.dbContext);
        }
    }
    public interface IDocumentManager : IManagerBase<Document>
    {

    }
}
