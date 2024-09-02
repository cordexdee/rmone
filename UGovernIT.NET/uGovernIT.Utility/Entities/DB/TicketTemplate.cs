using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.TicketTemplates)]
    public class TicketTemplate:DBBaseEntity
    {
        public long ID { get; set; }
        public string Description { get; set; }
        public string FieldValues { get; set; }
        public string ModuleNameLookup { get; set; }
        public string TemplateType { get; set; }
        public string Title { get; set; }
        [NotMapped]
        public string ModuleDescription { get; set; }

        
    }
}
