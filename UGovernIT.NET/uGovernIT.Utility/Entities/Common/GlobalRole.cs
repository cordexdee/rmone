using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.GlobalRole)]
    [JsonObject(MemberSerialization.OptOut)]
    public class GlobalRole : DBBaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FieldName { get; set; }
        public string Description { get; set; }

        public double BillingRate { get; set; }
    }
}
