using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace sLite
{
    [Serializable]
    public class Column
    {
        [XmlAttribute]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
    }
}
