using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace uGovernIT.Utility
{
    [DataContract]
    public class SPFieldLookupValue
    {
        [DataMember]
        public int LookupId { get; set; }
    }
}