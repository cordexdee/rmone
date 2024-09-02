using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL.Store
{
    public class DMDocumentTypeListStore : StoreBase<DMDocumentTypeList>
    {
        public DMDocumentTypeListStore(CustomDbContext context) : base(context)
        {

        }

    }
    public interface IDMDocumentTypeList:IStore<DMDocumentTypeList>
    {

    }
}
