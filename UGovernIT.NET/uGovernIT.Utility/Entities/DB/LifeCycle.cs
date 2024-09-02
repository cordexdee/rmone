using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ProtoBuf;

namespace uGovernIT.Utility
{
    [Serializable]
    [Table("Config_ModuleLifeCycles")]
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class LifeCycle:DBBaseEntity
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ItemOrder { get; set; }
        [NotMapped]       
        public List<LifeCycleStage> Stages { get; set; }
        //public bool IsDeleted { get; set; }
        public string ModuleNameLookup { get; set; }
    }
}
