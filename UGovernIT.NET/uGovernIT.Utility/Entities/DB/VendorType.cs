using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.VendorType)]
    public class VendorType:DBBaseEntity
    {
        public long id { get; set; }
        public string Title { get; set; }
        public string VTDescription { get; set; }
        //public bool isdeleted { get; set; }
    }
}
