using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    public  class ResetPasswordAgent
    {
        public bool IsResetPasswordAgentActivated { get; set; }
        public bool IsRequestorIsGroup { get; set; }
        public bool IsInitiatorEqualRequestor { get; set; }
        public string requestors { get; set; }
    }
}
