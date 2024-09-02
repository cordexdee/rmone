using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uGovernIT.Web.Models
{
    public class AllocationOverlapRequest
    {
        public long ID { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public string AssignedTo { get; set; }
        public string Type { get; set; }
    }
}