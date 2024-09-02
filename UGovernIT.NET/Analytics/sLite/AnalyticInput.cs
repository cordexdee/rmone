using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace sLite
{
    public class AnalyticInput
    {
        [XmlAttribute]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<FunctionInput> Functions { get; set; }
        public List<ExternalData> ExternalDataList { get; set; }
        public string KeyField { get; set; }
    }
}
