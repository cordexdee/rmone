using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL
{
    public class StudioStore : StoreBase<Studio>, IStudioStore
    {
        public StudioStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IStudioStore : IStore<Studio>
    {

    }
}
