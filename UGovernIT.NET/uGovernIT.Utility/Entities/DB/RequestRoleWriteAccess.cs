using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.DB
{
    [Table(DatabaseObjects.Tables.RequestRoleWriteAccess)]
    public class ModuleRequestRoleWriteAccess:DBBaseEntity
    {
        public long ID { get; set; }
        public string ActionUser { get; set; }
        public string CustomProperties { get; set; }
        public bool? FieldMandatory { get; set; }
        public string FieldName { get; set; }
        public bool? HideInServiceMapping { get; set; }
        public string ModuleNameLookup { get; set; }
        public int? StageStep { get; set; }
        public bool? ShowEditButton { get; set; }
        public bool? ShowWithCheckBox { get; set; }
        public string Title { get; set; }      
    }

}
