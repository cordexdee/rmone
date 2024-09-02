using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
namespace uGovernIT.Manager
{
    public class DrqSyetemAreaViewManager:ManagerBase<DRQSystemArea>,IDrqSyetemAreaViewManager
    {
        public DrqSyetemAreaViewManager(ApplicationContext context):base(context)
        {
            store = new DrqSystemAreaStore(this.dbContext);

        }
        
    }
    public interface IDrqSyetemAreaViewManager : IManagerBase<DRQSystemArea>
    {

    }
}
