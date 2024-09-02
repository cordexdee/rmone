using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.DataTransfer.Infratructure
{
    public class MappedItem
    {
        public string SourceID { get; set; }
        public string SourceRefFieldValue { get; set; }
        public object Source { get; set; }
       
        public string TargetID { get; set; }
        public string TargetRefFieldValue { get; set; }
        public object Target { get; set; }

        public MappedItem(string sourceid, string targetid)
        {
            SourceID = sourceid;
            TargetID = targetid;
        }
    }
}
