using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace uGovernIT.Utility
{
    [DataContract]
    public class SPLookupValueServiceExtension
    {
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string ListName { get; set; }

        public SPLookupValueServiceExtension(string id, string value, string listName)
        {
            ID = id;
            Value = value;
            ListName = listName;
        }
        public void AddIn(List<SPLookupValueServiceExtension> lookups)
        {
            if (lookups == null)
                return;

            if (!lookups.Exists(x => x.ID == this.ID && x.ListName == this.ListName))
                lookups.Add(this);
        }
    }
}