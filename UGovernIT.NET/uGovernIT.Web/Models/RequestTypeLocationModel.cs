using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uGovernIT.Web.Models
{
    public class RequestTypeLocationModel
    {
        public long LocationID { get; set; }
        public string Owner { get; set; }
        public string PRPGroup { get; set; }
        public string ORP { get; set; }
    }
}