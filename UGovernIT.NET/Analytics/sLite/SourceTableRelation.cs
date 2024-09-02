using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace sLite
{
    [Serializable]
    public class Relation
    {
        public String FirstTable { get; set; }
        public String FirstTableColumn { get; set; }
        public String SecondTable { get; set; }
        public String SecondTableColumn { get; set; }
    }
}
