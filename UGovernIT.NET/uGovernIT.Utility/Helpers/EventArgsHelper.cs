using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Helpers
{
    public class EventArgsClosePopupHandler : EventArgs
    {
        public int orgID { get; set; }
    }
    public class EventArgsSubmitEventHandler:EventArgs
    {
        public  string Value { get; set; }
    }
    public class EventArgsDoneClickEventHandler : EventArgs
    {
        public Dictionary<string, string> Dict { get; set; }
    }
}
