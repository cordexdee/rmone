using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM.ViewModel
{
    public class BillingAndMarginsResponse
    {
        public string Month { get; set; }
        public int MonthOrder { get; set; }
        public string TotalBillingLaborRate { get; set; }
        public string TotalEmployeeCostRate { get; set; }
        public int TotalProjects { get; set; }
        public string TotalCapacity { get; set; }
        public String GrossMargin { get; set; }

        public int BilledResources { get; set; }
        public int UnbilledResources { get; set; }
        public string MissedRevenues { get; set; }

        public string BilledWorkMonth { get; set; }
        public string UnBilledWorkMonth { get; set; }
        public string Utilization { get; set; }
    }
}
