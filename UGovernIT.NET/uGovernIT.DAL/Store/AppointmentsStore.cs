using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class AppointmentsStore : StoreBase<Appointment>, IAppointmentsStore
    {
        public AppointmentsStore(CustomDbContext context) : base(context)
        {
        }

    }
    public interface IAppointmentsStore : IStore<Appointment>
    {

    }
}
