using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ProjectInitiative)]
    public class ProjectInitiative:DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        //public bool IsDeleted { get; set; }
        public string ProjectNote { get; set; }
        public long BusinessStrategyLookup { get; set; }
        [NotMapped]
        public string BusinessStrategy { get; set; }
    }
}
