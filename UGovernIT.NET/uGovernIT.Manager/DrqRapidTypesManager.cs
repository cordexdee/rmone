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
    public class DrqRapidTypesManager:ManagerBase<DRQRapidType>, IDRQRapidTypes
    {
        public DrqRapidTypesManager(ApplicationContext context):base(context)
        {
            store = new DrqRapidTypesStore(this.dbContext);
        }
    }
    public interface IDRQRapidTypes : IManagerBase<DRQRapidType>
    {
    }
}
