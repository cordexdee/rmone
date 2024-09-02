using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.DB
{
    [Table(DatabaseObjects.Tables.DMDocumentTypeList)]
    public  class DMDocumentTypeList :DBBaseEntity
    {
        public long ID { get; set; }
        public string DMSDescription { get; set; }
        public string Title { get; set; }
    }
}
