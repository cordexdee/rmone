using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [DataContract]
    public class SPQuestionMapVariable
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string Desc { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public List<SPVariableValue> VariableValues { get; set; }
        [DataMember]
        public SPVariableValue DefaultValue { get; set; }

        public SPQuestionMapVariable()
        {
            ID = Guid.NewGuid();
            ShortName = string.Empty;
            Title = string.Empty;
            Desc = string.Empty;
            VariableValues = new List<SPVariableValue>();
            DefaultValue = new SPVariableValue();
        }
    }
}
