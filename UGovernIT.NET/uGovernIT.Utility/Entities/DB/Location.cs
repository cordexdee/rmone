using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.Location)]
    public class Location:DBBaseEntity
    {
        public long ID { get; set; }
        public string Country { get; set; }
        public string LocationDescription { get; set; }
        public string Region { get; set; }
        public string State { get; set; }
        public string Title { get; set; }       
        //public bool IsDeleted { get; set; }
       
        
    }
}
