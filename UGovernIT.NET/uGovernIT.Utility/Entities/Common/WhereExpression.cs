using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace uGovernIT.Utility
{
    [DataContractAttribute]
    public class WhereExpression
    {
        [DataMember]
        public string Variable { get; set; }
        [DataMember]
        public string Operator { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string ValueForWF { get; set; }
        [DataMember]
        public string AppendWith { get; set; }

        /// <summary>
        /// This operator is used to keep the value of Logical Relational Operators applied between multiple questions for a skip condition
        /// </summary>
        [DataMember]
        public string LogicalRelOperator { get; set; }

        /// <summary>
        /// This operator is used to keep the value of item ID
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// This operator is used to keep the value of ParentId i.e. the Id of first grouping item
        /// </summary>
        [DataMember]
        public int ParentId { get; set; }
    }
}
