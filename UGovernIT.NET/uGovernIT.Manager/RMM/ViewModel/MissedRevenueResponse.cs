using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM.ViewModel
{
    public class MissedRevenueResponse
    {
        public string MonthName { get; set; }
        public int MonthOrder { get; set; }
        public int ResourceNotBilled { get; set; }
        public string TotalMissedBilling { get; set; }
        public string TotalMissedCost { get; set; }
        public string GrossMargin { get; set; }
    }
}
