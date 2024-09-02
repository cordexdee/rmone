using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [DataContract]
    public class SPServiceSection
    {
        [DataMember]
        public long ID { get; private set; }
        [DataMember]
        public long ServiceID { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string SectionTitle { get; set; }
        [DataMember]
        public int ItemOrder { get; set; }

        [DataMember]
        public string Description { get; set; }

        private SPServiceSection()
        {
            SectionTitle = string.Empty;
            ServiceName = string.Empty;
            Description = string.Empty;
        }

        public SPServiceSection(int serviceID)
            : base()
        {
            ServiceID = serviceID;
        }
    }
}
