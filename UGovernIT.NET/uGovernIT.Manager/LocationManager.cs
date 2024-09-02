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
   public class LocationManager:ManagerBase<Location>, ILocationManager
    {
        public LocationManager(ApplicationContext context):base(context)
        {
            store = new LocationStore(this.dbContext);
        }

        public Location AddOrUpdate(Location location)
        {
            if (location != null)
            {
                if (location.ID > 0)
                    this.Update(location);
                else
                    this.Insert(location);
            }
            return location;
        }

    }
    public interface ILocationManager : IManagerBase<Location>
    {
        Location AddOrUpdate(Location location);

    }
}
