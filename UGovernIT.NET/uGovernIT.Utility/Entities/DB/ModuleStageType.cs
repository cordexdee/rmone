using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.StageType)]
    public class Module_StageType:DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string ModuleStageType { get; set; }
    }
}
