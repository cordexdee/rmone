using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace sLite
{
    public class DataIntegration
    {
        [XmlAttribute]
        public string ID { get; set; }

        public string IntegrationID { get; set; }
        public string ListName { get; set; }
        public string ColumnName { get; set; }
        public string Mode { get; set; }
    }
}
