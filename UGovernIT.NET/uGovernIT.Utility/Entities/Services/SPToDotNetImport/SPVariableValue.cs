using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [DataContract]
    public class SPVariableValue
    {
        [DataMember]
        public List<SPWhereExpression> Conditions { get; set; }
        [DataMember]
        public string PickFrom { get; set; }
        [DataMember]
        public bool IsPickFromConstant { get; set; }
        [DataMember]
        public bool IsValid { get; set; }

        public SPVariableValue()
        {
            PickFrom = string.Empty;
        }
    }
}
