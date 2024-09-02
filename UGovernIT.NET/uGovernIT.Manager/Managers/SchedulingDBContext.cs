using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public partial class SchedulingDBContext
    {
        public SchedulingDBContext()
        {

        }
      
        public ISet<Appointment> Appointments { get; set; }
    }
}
