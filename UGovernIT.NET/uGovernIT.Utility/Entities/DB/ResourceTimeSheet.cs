using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ResourceTimeSheet)]
    public class ResourceTimeSheet:DBBaseEntity
    {
        public long ID { get; set; }
        public double? HoursTaken { get; set; }
        [Column(DatabaseObjects.Columns.Resource)]
        public string Resource { get; set; }
        [NotMapped]
        public  ResourceWorkItems ResourceWorkItem { get; set; }
        public long ResourceWorkItemLookup { get; set; }
        public DateTime? WorkDate { get; set; }
        public string WorkDescription { get; set; }
        public string Title { get; set; }
        public ResourceTimeSheet()
        {
          
        }
        public ResourceTimeSheet(string userId, DateTime wDate)
        {
            Resource = userId;
            WorkDescription = string.Empty;
            WorkDate = wDate;
        }
    }

}
