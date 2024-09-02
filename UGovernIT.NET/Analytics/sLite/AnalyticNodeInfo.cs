using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace sLite
{
    public class AnalyticNodeInfo
    {
        [XmlAttribute]
        public Guid ID { get; set; }
        [XmlAttribute]
        public bool MainNode { get; set; }
        public string Help { get; set; }
        public bool EnableIgnore { get; set; }
        public List<DataIntegration> DataIntegrations { get; set; }
        public Survey Survey { get; set; }
        public List<AnalyticFormula> Formulas { get; set; }
        public List<Information> InformationNode { get; set; }

    }
}
