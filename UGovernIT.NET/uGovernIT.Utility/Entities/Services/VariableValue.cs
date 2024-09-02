using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace uGovernIT.Utility
{
    [DataContractAttribute]
    public class VariableValue
    {
        [DataMember]
        public List<WhereExpression> Conditions { get; set; }
        [DataMember]
        public string PickFrom { get; set; }
        [DataMember]
        public bool IsPickFromConstant { get; set; }
        [DataMember]
        public bool IsValid { get; set; }

        public VariableValue()
        {
            PickFrom = string.Empty;
        }
    }
}
