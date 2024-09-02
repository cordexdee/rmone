using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [DataContract]
    public class SPServiceQuestionMapping
    {
        [DataMember]
        public int ID { get; private set; }
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public int ServiceQuestionID { get; set; }
        [DataMember]
        public string ServiceQuestionName { get; set; }
        [DataMember]
        public int ServiceTaskID { get; set; }
        [DataMember]
        public string ServiceTaskName { get; set; }

        [DataMember]
        public string ColumnName { get; set; }
        [DataMember]
        public string ColumnValue { get; set; }
        [DataMember]
        public string PickValueFrom { get; set; }

    }
}
