using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace sLite
{
    [Serializable]
    public class Analytic
    {
        [XmlAttribute]
        public Guid ID { get; set; }

        public string Name { get; set; }
        public List<KPI> KPIs { get; set; }
        public float Weight { get; set; }

    }
}
