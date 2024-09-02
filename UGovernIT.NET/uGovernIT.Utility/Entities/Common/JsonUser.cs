using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace uGovernIT.Utility
{
    public class JsonUserList
    {
        public List<JsonUser> Users { get; set; }
        public JsonUserList()
        {
            Users = new List<JsonUser>();
        }

        public string Serialized(List<JsonUser> users)
        {
            JsonSerializer json = new JsonSerializer();
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);
            json.Serialize(writer, users);
            return sb.ToString();
        }
    }

    public class JsonUser
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string picture { get; set; }
    }
}
