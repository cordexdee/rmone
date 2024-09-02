using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager
{
    
    public class RequestRoleWriteAccessManager : ManagerBase<ModuleRoleWriteAccess>, IRequestRoleWriteAccessManager
    {
        public RequestRoleWriteAccessManager(ApplicationContext context) : base(context)
        {
            store = new RequestRoleWriteAccessStore(this.dbContext);
        }
        public ModuleRoleWriteAccess AddOrUpdate(ModuleRoleWriteAccess item)
        {
            if (item.ID > 0)
                this.Update(item);
            else
                this.Insert(item);
            return item;
        }
    }
    public interface IRequestRoleWriteAccessManager : IManagerBase<ModuleRoleWriteAccess>
    {

    }
}
