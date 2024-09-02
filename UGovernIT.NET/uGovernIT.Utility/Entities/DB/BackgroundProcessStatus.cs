using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.DB
{
    [Table(DatabaseObjects.Tables.BackgroundProcessStatus)]
    public class BackgroundProcessStatus : DBBaseEntity
    {
        public long ID { get; set; }
        public string ServiceName { get; set; }
        //public bool IsDeleted { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Status { get; set; }
        public string UserName { get; set; }

    }
}
