using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.SubContractor)]
    public class SubContractor
    {
        public long ID { get; set; }
        public string CommittmentID { get; set; }
        public string CommittmentNumber { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string ExternalProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
        //public bool IsDeleted { get; set; }

  
    }
}
