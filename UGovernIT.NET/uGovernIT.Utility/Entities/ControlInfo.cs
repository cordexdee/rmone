using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class ControlsInfo
    {
       public List<ControlInfo> controlInfo { get; set; }
    }
    public class ControlInfo
  {
        public string AssemblyName { get; set; }
        public TicketDockConfiguration ticketDockConfiguration { get; set; }
        public DashboardDockConfiguration dashboardConfiguration { get; set; }

    }
}
