using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace sLite
{
    [Serializable]
    public class Rule
    {
        [XmlAttribute]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public String FirstCondition { get; set; }
        public String SecondCondition { get; set; }
        public String FirstConditionOp { get; set; }
        public String SecondConditionOp { get; set; }
        public double MaxInput { get; set; }
        public double MinInput { get; set; }
        public String FirstConditionVal { get; set; }
        public String SecondConditionVal { get; set; }
        public String FirstConditionOtherVal { get; set; }
        public String SecondConditionOtherVal { get; set; }
        public string IfExpression { get; set; }
        
    }
}
