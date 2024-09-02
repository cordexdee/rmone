using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{  
    [Table(DatabaseObjects.Tables.GovernanceLinkItems)]
    public class GovernanceLinkItem:DBBaseEntity
    {
        public long ID { get; set; }
        public string CustomProperties { get; set; }
        public string Description { get; set; }
        public long GovernanceLinkCategoryLookup { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public int? TabSequence { get; set; }
        public string TargetType { get; set; }
        public string Title { get; set; }       
    }

}
