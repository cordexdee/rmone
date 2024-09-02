using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;
using static uGovernIT.Manager.SubLocationManager;

namespace uGovernIT.Manager
{
    public class SubLocationManager: ManagerBase<SubLocation>, ISubLocationManager
    {
        public SubLocationManager(ApplicationContext context) : base(context)
        {
            store = new SubLocationStore(this.dbContext);
        }
        public SubLocation AddOrUpdate(SubLocation location)
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
        public interface ISubLocationManager : IManagerBase<SubLocation>
        {
            SubLocation AddOrUpdate(SubLocation location);

        }

        public SubLocation GetDataTable(int id)
        {
            throw new NotImplementedException();
        }

        public void RefreshList(string list)
        {
            throw new NotImplementedException();
        }
    }
}

