using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class DrqRapidTypesStore:StoreBase<DRQRapidType>, IDrqRapidTypesStore
    {
        public DrqRapidTypesStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IDrqRapidTypesStore : IStore<DRQRapidType>
    {
    }
}
