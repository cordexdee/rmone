using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [DataContractAttribute]
    public class QuestionMapVariable
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
        public List<VariableValue> VariableValues { get; set; }
        [DataMember]
        public VariableValue DefaultValue { get; set; }

        public QuestionMapVariable()
        {
            ID = Guid.NewGuid();
            ShortName = string.Empty;
            Title = string.Empty;
            Desc = string.Empty;
            VariableValues = new List<VariableValue>();
            DefaultValue = new VariableValue();
        }
    }
}
