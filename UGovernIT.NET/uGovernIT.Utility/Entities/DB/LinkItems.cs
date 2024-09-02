using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{   
    [Table(DatabaseObjects.Tables.LinkItems)]
    public class LinkItems:DBBaseEntity
    {
        public long ID { get; set; }
        public string AuthorizedToView { get; set; }
        public string CustomProperties { get; set; }
        public string Description { get; set; }
        public int? ItemOrder { get; set; }
        public long LinkCategoryLookup { get; set; }
        public string TargetType { get; set; }
        public string Title { get; set; }
        [NotMapped]
        public string LinkCategory { get; set; }

        public bool DisableDiscussion { get; set; }
        public bool DisableRelatedItems { get; set; }
        public bool LeftPaneExpanded { get; set; }  
        public bool BottomPaneExpanded { get; set; }  
    }

}
