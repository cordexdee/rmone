using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace sLite
{
    public class AnalyticFormula
    {
    
        public Guid ID { get; set; }
        public string FName { get; set; }
        public string ShortName { get; set; }
        public string Expression { get; set; }
        public string TableName { get; set; }
        public string IntegrationID { get; set; }
        public string FormulaType { get; set; }
    }
}
