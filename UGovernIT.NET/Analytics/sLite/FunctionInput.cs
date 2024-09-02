using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace sLite
{
    public class FunctionInput
    {
        [XmlAttribute]
        public Guid ID { get; set; }
        [XmlAttribute]
        public bool Ignore { get; set; }
        public object Value { get; set; }
        public string Table { get; set; }
        public bool IsMultiValue { get; set; }
        public List<FunctionValue> functionValues { get; set; }
    }
}
