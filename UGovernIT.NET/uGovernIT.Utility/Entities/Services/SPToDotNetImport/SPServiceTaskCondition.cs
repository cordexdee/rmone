using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [DataContract]
    public class SPServiceTaskCondition
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Condition { get; set; }
        [DataMember]
        public List<SPWhereExpression> Conditions { get; set; }
        [DataMember]
        public List<int> SkipTasks { get; set; }

        public SPServiceTaskCondition()
        {
            ID = Guid.NewGuid();
            Title = string.Empty;
            Condition = string.Empty;
            SkipTasks = new List<int>();
            Conditions = new List<SPWhereExpression>();
        }
    }
}
