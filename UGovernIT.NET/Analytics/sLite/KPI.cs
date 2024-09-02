using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace sLite
{
    public class KPI
    {
        [XmlAttribute]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Attachment { get; set; }
        public List<Metric> Metrics { get; set; }
        public float Weight { get; set; }
    }
}
