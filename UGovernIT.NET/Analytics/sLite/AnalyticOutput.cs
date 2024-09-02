using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace sLite
{
    public class AnalyticOutput
    {
        [XmlAttribute]
        public Guid ID
        {
            get;
            set;
        }

        public float Weight
        {
            get;
            set;

        }

        public float CalculatedWeight
        {
            get;

            set;

        }

        public double Score
        {
            get;
            set;
        }

        public double CalculatedScore
        {
            get;
            set;
        }

        public string RunName { get; set; }
        public string Name { get; set; }

        public string KeyField { get; set; }

        public List<KPIOutput> KPIs { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }

    }
}
