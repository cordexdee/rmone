using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL
{
    public class BusinessStrategyStore : StoreBase<BusinessStrategy>, IBusinessStrategyStore
    {
        public BusinessStrategyStore(CustomDbContext context) : base(context){

        }
    }
    public interface IBusinessStrategyStore : IStore<BusinessStrategy>
    {

    }
}
