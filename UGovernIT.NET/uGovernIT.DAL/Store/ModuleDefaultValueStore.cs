using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL
{
    public class ModuleDefaultValueStore : StoreBase<ModuleDefaultValue>, IModuleDefaultValueStore
    {
        public ModuleDefaultValueStore(CustomDbContext context) : base(context)
        {

        }
    }

    public interface IModuleDefaultValueStore : IStore<ModuleDefaultValue>
    {

    }

}
