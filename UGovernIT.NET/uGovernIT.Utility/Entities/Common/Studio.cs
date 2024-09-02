using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.Studio)]
    [JsonObject(MemberSerialization.OptOut)]
    public class Studio : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long DivisionLookup { get; set; }
        public new bool Deleted { get; set; }
        public string FieldDisplayName { get; set; }
        [NotMapped]
        public string Division { get; set; }
    }
}
