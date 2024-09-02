using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM.ViewModel
{
    public class ExecutiveKpi
    {
        public string Category { get; set; }
        public string TicketTitle { get; set; }
        public string ProjectId { get; set; }
        public string Margins { get; set; }
        public string ProjectedMargins { get; set; }
        public string ProjectMargins { get; set; }
        public string EffectiveUtilization { get; set; }
        public string CommittedUtilization { get; set; }
        public string PipelineUtilization { get; set; }

        public string RevenuesRealized { get; set; }
        public string RevenuesLost { get; set; }
        public string CommittedRevenues { get; set; }
        public string PipelineRevenues { get; set; }

        public string MarginsRealized { get; set; }
        public string MarginsLost { get; set; }
        public string CommittedMargins { get; set; }

        public string CurrentBillings { get; set; }
        public string CurrentResourceCosts { get; set; }
        public string ResourceHours { get; set; }
        public string ResourceBillings { get; set; }
        public string ResourceCosts { get; set; }
        public string ResourceMargins { get; set; }
        public string UtilizationRate { get; set; }
        
    }
}
