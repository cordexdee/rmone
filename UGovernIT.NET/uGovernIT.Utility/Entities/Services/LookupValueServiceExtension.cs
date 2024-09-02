using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    [DataContract]
    public class LookupValueServiceExtension
    {
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string ListName { get; set; }

        public LookupValueServiceExtension(string id, string value, string listName)
        {
            ID = id;
            Value = value;
            ListName = listName;
        }
        public void AddIn(List<LookupValueServiceExtension> lookups)
        {
            if (lookups == null)
                return;

            if (!lookups.Exists(x => x.ID == this.ID && x.ListName == this.ListName))
                lookups.Add(this);
        }
    }
}
