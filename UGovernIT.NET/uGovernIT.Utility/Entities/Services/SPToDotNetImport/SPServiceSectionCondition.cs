using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [DataContract]
    public class SPServiceSectionCondition
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string ConditionVar { get; set; }
        [DataMember]
        public string ConditionOperator { get; set; }
        [DataMember]
        public string ConditionVal { get; set; }
        [DataMember]
        public string ConditionValForWF { get; set; }
        [DataMember]
        public string Condition { get; set; }
        [DataMember]
        public List<SPWhereExpression> Conditions { get; set; }
        [DataMember]
        public List<int> SkipSectionsID { get; set; }
        [DataMember]
        public List<int> SkipQuestionsID { get; set; }
        [DataMember]
        public bool ConditionValidate { get; set; }

        public SPServiceSectionCondition()
        {
            ID = Guid.NewGuid();
            Title = string.Empty;
            ConditionVal = string.Empty;
            ConditionVar = string.Empty;
            ConditionOperator = string.Empty;
            SkipSectionsID = new List<int>();
            SkipQuestionsID = new List<int>();
            Condition = string.Empty;

            Conditions = new List<SPWhereExpression>();
        }
    }
}
