using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    public  class TenantConstraints
    {
        public int TenantCountCritical;
        public int TenantCountHigh;
        public int TicketCountCritical;
        public int TicketCountHigh;
        public int ServiceCountCritical;
        public int ServiceCountHigh;

    }

    public class TenantStatistics
    {
       
        public int TicketCount;
        public string TicketClass;
        public int ServiceCount;
        public string ServiceClass;
    }
}
