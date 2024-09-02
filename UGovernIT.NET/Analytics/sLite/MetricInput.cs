using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace sLite
{
    public class MetricInput
    {
        [XmlAttribute]
        public Guid ID { get; set; }
        public List<FunctionInput> Functions { get; set; }
    }
}

