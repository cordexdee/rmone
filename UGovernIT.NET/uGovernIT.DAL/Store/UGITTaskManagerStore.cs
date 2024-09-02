using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class UGITTaskManagerStore:StoreBase<UGITTask>, IUGITTaskHelperStore
    {
        public UGITTaskManagerStore(CustomDbContext context):base(context)
        {
            

        }
    }
    public interface IUGITTaskHelperStore : IStore<UGITTask>
    {
    }
}
