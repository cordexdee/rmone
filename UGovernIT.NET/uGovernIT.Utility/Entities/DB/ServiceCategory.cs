using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ServiceCategories)]
    public class ServiceCategory:DBBaseEntity
    {
        public long ID { get;  set; }
        public string CategoryName { get; set; }
        public int ItemOrder { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
    }
}
