using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace uGovernIT.Utility
{
 
    [Table(DatabaseObjects.Tables.RequestRoleWriteAccess)]
    [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class ModuleRoleWriteAccess:DBBaseEntity
    {
        public long ID { get; set; }
        public string ActionUser { get; set; }
        //Custom Properties
        public string CustomProperties { get; set; }
        public bool FieldMandatory { get; set; }
        public string FieldName { get; set; }
        public bool HideInServiceMapping { get; set; }
        public string ModuleNameLookup { get; set; }
        public int StageStep { get; set; }
        [NotMapped]
        public int StageStepLookup { get; set; }
        public bool ShowEditButton { get; set; }
        public bool ShowWithCheckbox { get; set; }
        public string Title { get; set; }
    }
}
