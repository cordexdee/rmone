using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace sLite
{
    [Serializable]
   public  class Information
    {
        public String Question { get; set; }
        public String Answer { get; set; }
        public String ShortName { get; set; }
        public List<FunctionValue> Options { get; set; }
    }
}
