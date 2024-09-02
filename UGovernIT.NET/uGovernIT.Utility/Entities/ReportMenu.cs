using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.ReportMenu)]
    public class ReportMenu  : DBBaseEntity
    {

        public long ID { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string RouteUrl { get; set; }
        public string ImageUrl { get; set; }
        public string ModuleNameLookup { get; set; }
        public string Category { get; set; }
        [NotMapped]
        public string ModuleShortName { get; set; }

        //public bool IsDeleted {get; set;}

        [NotMapped]
        public string LongTitle { get; set; }

    }
}
