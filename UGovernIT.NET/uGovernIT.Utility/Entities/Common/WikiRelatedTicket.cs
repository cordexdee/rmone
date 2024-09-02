using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    public class WikiRelatedTicket
    {
        public long ID { get; set; }
        public string ParentModuleName { get; set; }
        public string ParentTicketID { get; set; }
        public string ChildModuleName { get; set; }
        public string ChildTicketID { get; set; }
        public string ChildTicketTitle { get; set;}
        public string Link { get; set; }
    }
}
