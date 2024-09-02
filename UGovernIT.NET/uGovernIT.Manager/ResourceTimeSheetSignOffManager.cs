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
    public class ResourceTimeSheetSignOffManager : ManagerBase<ResourceTimeSheetSignOff>, IResourceTimeSheetSignOffManager
    {
        public ResourceTimeSheetSignOffManager(ApplicationContext context) : base(context)
        {
            store = new ResourceTimeSheetSignOffStore(this.dbContext);
        }

        public ResourceTimeSheetSignOff AddOrUpdate(ResourceTimeSheetSignOff resourceTimeSheetSignOff)
        {
            if (resourceTimeSheetSignOff != null)
            {
                if (resourceTimeSheetSignOff.ID > 0)
                    this.Update(resourceTimeSheetSignOff);
                else
                    this.Insert(resourceTimeSheetSignOff);
            }
            return resourceTimeSheetSignOff;
        }

    }
    public interface IResourceTimeSheetSignOffManager : IManagerBase<ResourceTimeSheetSignOff>
    {
        ResourceTimeSheetSignOff AddOrUpdate(ResourceTimeSheetSignOff resourceTimeSheetSignOff);
    }
}
