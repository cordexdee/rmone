using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ConfigurationVariableStore:StoreBase<ConfigurationVariable>, IConfigurationVariableStore
    {
        public ConfigurationVariableStore(CustomDbContext context):base(context)
        {


        }

    }
    public interface IConfigurationVariableStore:IStore<ConfigurationVariable>
    { }
}
