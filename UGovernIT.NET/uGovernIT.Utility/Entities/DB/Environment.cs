using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.Environment)]
    public class UGITEnvironment  : DBBaseEntity
    {
        public long ID { get; set; }
        public string Description { get; set; }
        //public bool IsDeleted { get; set; }
        public string Title { get; set; }
    }
}
