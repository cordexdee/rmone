using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace uGovernIT.Utility
{
    [DataContract]
    public class ServiceRequestDTO
    {
        [DataMember]
        public long ServiceId { get; set; }

        [DataMember]
        public List<QuestionsDTO> Questions { get; set; }

        public ServiceRequestDTO()
        {
            Questions = new List<QuestionsDTO>();
        }
    }

    [DataContract]
    public class QuestionsDTO
    {
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public string Value { get; set; }

        public List<QuestionsDTO> SubTokensValue;

        public QuestionsDTO()
        {
            Token = string.Empty;
            Value = string.Empty;
            SubTokensValue = new List<QuestionsDTO>();
        }
    }
}
