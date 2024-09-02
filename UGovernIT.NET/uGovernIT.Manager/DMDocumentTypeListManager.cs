using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager
{
    public class DMDocumentTypeListManager:ManagerBase<DMDocumentTypeList>
    {
        public DMDocumentTypeListManager(ApplicationContext context) : base(context)
        {
            store = new DMDocumentTypeListStore(this.dbContext);
        }
    }

    public interface IDMDocumentTypeListManager:IManagerBase<DMDocumentTypeList>
    {

    }
}
