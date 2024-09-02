using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM.ViewModel
{
    public class BillingAndMarginsRequest
    {
         public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Mode { get; set; }
    }
}
