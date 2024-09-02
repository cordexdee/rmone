using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.DB
{
    [Table(DatabaseObjects.Tables.WikiMenuLeftNavigation)]
    public class WikiMenuLeftNavigation: DBBaseEntity
    {
        public long ID { get; set; }
        public string ColumnType { get; set; }
       // public string ConditionalLogic { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public int? ItemOrder { get; set; }

    }
}
