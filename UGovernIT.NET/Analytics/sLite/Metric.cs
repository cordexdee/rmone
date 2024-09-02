using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace sLite
{
  
    public class Metric
    {
        [XmlAttribute]
        public Guid ID { get; set; }
        public string Name { get; set; }

        [XmlArray]
        [XmlArrayItem(Type = typeof(LabelFunction))]
        [XmlArrayItem(Type = typeof(TableFunction))]
        [XmlArrayItem(Type = typeof(SimpleFunction))]
        [XmlArrayItem(Type = typeof(ConditionalFunction))]
        [XmlArrayItem(Type = typeof(NonLinearFunction))]
        [XmlArrayItem(Type = typeof(MapFunction))]
        public List<Function> Functions { get; set; }
        public float Weight { get; set; }
    }
}
