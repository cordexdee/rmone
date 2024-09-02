using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ACRTypeStore : StoreBase<ACRType>, IACRTypeStore
    {
        public ACRTypeStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IACRTypeStore : IStore<ACRType>
    {

    }
}
