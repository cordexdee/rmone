using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class SLAConfiguration
    {
        public bool EnableEscalation { get; set; }
        public double EscalationAfter { get; set; }
        public double EscalationFrequency { get; set; }
        public string EscalationTo { get; set; }
        public string EscalationEmailTo { get; set; }
        
        /// <summary>
        /// Escalation unit has after/before
        /// Value will be 0/1 
        /// after=0,before=1
        /// </summary>
        public string EscalationUnit { get; set; }
    }
}
