using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.PMMComments)]
    public class PMMComments:DBBaseEntity
    {
        public long ID { get; set; }
        public DateTime? AccomplishmentDate { get; set; }
        public DateTime? EndDate { get; set; }
        //public bool? Deleted { get; set; }
        public long PMMIdLookup { get; set; }
        public string ProjectNote { get; set; }
        public string ProjectNoteType { get; set; }
        public string Title { get; set; }
        //public string TenantID { get; set; }
        //public bool IsDeleted { get; set; }
        public string TicketId { get; set; }
        [NotMapped]
        public string PMMTitle { get; set; }
    }

}
