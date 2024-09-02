using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class DrqSystemAreaStore:StoreBase<DRQSystemArea>, IDrqSystemAreaStore
    {
        public DrqSystemAreaStore(CustomDbContext context):base(context)
        {


        }
    }
    public interface IDrqSystemAreaStore : IStore<DRQSystemArea>
    {

    }
}
