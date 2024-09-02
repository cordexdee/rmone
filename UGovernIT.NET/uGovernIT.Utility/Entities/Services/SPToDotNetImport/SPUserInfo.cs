using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace uGovernIT.Utility
{
    [DataContract]
    [Serializable]
    public class SPUserInfo
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public bool IsGroup { get; set; }
    }
}