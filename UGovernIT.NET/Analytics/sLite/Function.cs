using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace sLite
{
    [Serializable]
    public class Function
    {   
        [XmlAttribute]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public string AskFrom { get; set; }
        public double AskFromValue { get; set; }

        public virtual double Execute(FunctionInput functionInput, bool isNormalize,double normalizeBy)
        {
            return 0;
        }
    }
}
