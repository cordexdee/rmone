using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.DB
{
    [Table(DatabaseObjects.Tables.WikiReviews)]
    public class WikiReviews : DBBaseEntity
    {
       public long ID { get; set; }
       public string TicketId { get; set; }
       public ReviewType ReviewType { get; set; }
       public ReviewStatus ReviewStatus { get; set; }
       public double Rating { get; set; }
       public double Score { get; set; }

    }
}
