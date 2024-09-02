using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL.Store
{
    public class BackgroundProcessStatusStore : StoreBase<BackgroundProcessStatus>, IBackgroundProcessStatusStore
    {

        public BackgroundProcessStatusStore(CustomDbContext context) : base(context)
        {

        }

    }
    public interface IBackgroundProcessStatusStore : IStore<BackgroundProcessStatus>
    {

    }

}
