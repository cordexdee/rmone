using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
   public class ImpactManager:ManagerBase<ModuleImpact>,IImpactManager
    {
        public ImpactManager(ApplicationContext context):base(context)
        {
            store = new ModuleImpactStore(this.dbContext);
        }
    }
    public interface IImpactManager : IManagerBase<ModuleImpact>
    {

    }
}
