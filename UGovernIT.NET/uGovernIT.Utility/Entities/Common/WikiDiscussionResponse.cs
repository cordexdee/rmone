using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    public class WikiDiscussionResponse
    {
        public long ID { get; set; }
        public string Comment { get; set; }
        public string WikiTicketID { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
    }
}
