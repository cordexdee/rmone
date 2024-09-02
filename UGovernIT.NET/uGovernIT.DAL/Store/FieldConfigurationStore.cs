using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class FieldConfigurationStore : StoreBase<FieldConfiguration>, IFieldConfigurationStore
    {
        public FieldConfigurationStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface IFieldConfigurationStore : IStore<FieldConfiguration>
    {

    }
}
