using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    public class AddWikiRequestModel
    {
        public int RequestType { get; set; }
        public string WikiTitle { get; set; }
        public string HtmlBody { get; set; }
        public string Module { get; set; }
        public string TicketId { get; set;}
    }
}
