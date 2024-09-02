using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ServiceQuestionMappingStore : StoreBase<ServiceQuestionMapping>, IServiceQuestionMappingStore
    {
        public ServiceQuestionMappingStore(CustomDbContext context) : base(context)
        {
        }
    }
    interface IServiceQuestionMappingStore : IStore<ServiceQuestionMapping>
    {

    }
}
