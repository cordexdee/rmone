using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class ServiceQuestionStore : StoreBase<ServiceQuestion>, IServiceQuestionStore
    {
        public ServiceQuestionStore(CustomDbContext context) : base(context)
        {
        }
    }
    interface IServiceQuestionStore : IStore<ServiceQuestion>
    {

    }
}
