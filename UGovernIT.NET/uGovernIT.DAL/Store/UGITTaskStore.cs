using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class UGITTaskStore : StoreBase<UGITTask>, IUGITTaskStore
    {
        public UGITTaskStore(DbContext context):base(context)
        {

        }
    }
    public interface IUGITTaskStore : IStore<UGITTask>
    { }
}
