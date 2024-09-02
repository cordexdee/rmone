using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace uGovernIT.Utility
{
    [JsonObject(MemberSerialization.OptOut)]
    [Serializable, DataContract]
    //[Serializable]
    public abstract class DBBaseEntity
    {
        public string TenantID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        [Column("CreatedByUser")]
        public string CreatedBy { get; set; }
        [Column("ModifiedByUser")]
        public string ModifiedBy { get; set; }
        public string Attachments { get; set; }
        public bool Deleted { get; set; }
      
        
    }

}
