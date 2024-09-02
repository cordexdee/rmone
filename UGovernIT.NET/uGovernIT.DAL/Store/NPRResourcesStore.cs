using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class NPRResourcesStore:StoreBase<NPRResource>, INPRResourcesStore
    {
        public NPRResourcesStore(CustomDbContext context):base(context)
        {

        }
        public NPRResource InsertORUpdateData(NPRResource objNprResources)
        {
            if (objNprResources.ID > 0)
            {
               Update(objNprResources);
            }
            else
            {
                Insert(objNprResources);

            }
            return objNprResources;
        }

    }
    public interface INPRResourcesStore:IStore<NPRResource>
    {

    }
}
