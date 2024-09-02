using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ResourceWorkItems)]
    public class ResourceWorkItems:DBBaseEntity
    {
        public long ID { get; set; }
        [Column(DatabaseObjects.Columns.Resource)]
        public string Resource { get; set; }

        public string SubSubWorkItem { get; set; }//level4
        public string SubWorkItem { get; set; }//level3
        public string WorkItem { get; set; }//level2
        public string WorkItemType { get; set; }//level1
        public string Title { get; set; }
       
        //[NotMapped]
        public DateTime? StartDate { get; set; }
        //[NotMapped]
        public DateTime? EndDate { get; set; }

        public ResourceWorkItems(string userId)
        {
            WorkItemType = string.Empty;
            WorkItem = string.Empty;
            SubWorkItem = string.Empty;
            Resource = userId;
        }
        public ResourceWorkItems()
        {
         
        }
    }

}
