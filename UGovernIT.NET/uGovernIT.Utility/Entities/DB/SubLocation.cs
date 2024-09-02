using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.SubLocation)]
    public class SubLocation: DBBaseEntity
    {
        
        public long ID { get; set; }
        public long LocationID { get; set; }
        public string Title { get; set; }
        public string LocationTag { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public new bool Deleted { get; set; }
        [NotMapped]
        public List<string> LocationLookup { get; set; }
        [NotMapped]
        public string LocationDetails { get; set; }
    }
}
