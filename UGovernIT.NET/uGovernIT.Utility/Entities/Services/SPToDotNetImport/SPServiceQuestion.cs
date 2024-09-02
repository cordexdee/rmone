using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [DataContract]
    public class SPServiceQuestion
    {
        public SPServiceQuestion()
        {
            QuestionTitle = string.Empty;
            ServiceSectionName = string.Empty;
            Helptext = string.Empty;
            TokenName = string.Empty;
            QuestionType = string.Empty;
            NavigationUrl = string.Empty;

            QuestionTypeProperties = new Dictionary<string, string>();

        }

        public SPServiceQuestion(int serviceID)
            : base()
        {
            ServiceID = serviceID;

        }

        [DataMember]
        public int ID { get; private set; }
        [DataMember]
        public int ServiceSectionID { get; set; }
        [DataMember]
        public string ServiceSectionName { get; set; }
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string QuestionTitle { get; set; }
        [DataMember]
        public string QuestionType { get; set; }
        [DataMember]
        public Dictionary<string, string> QuestionTypeProperties { get; set; }
        [DataMember]
        public string Helptext { get; set; }
        [DataMember]
        public int ItemOrder { get; set; }
        [DataMember]
        public string TokenName { get; set; }
        [DataMember]
        public bool Mandatory { get; set; }
        [DataMember]
        public bool EnableZoomIn { get; set; }

        [DataMember]
        public string NavigationUrl { get; set; }

        [DataMember]
        public bool ContinueSameLine { get; set; }

    }
}
