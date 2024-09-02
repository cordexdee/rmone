using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class SurveyFeedback : DBBaseEntity 
    {
        public long ID { get; set; }
        public string Description { get; set; }
        [Column(DatabaseObjects.Columns.ModuleNameLookup)]
        public string ModuleName { get; set; }
        public int Rating1 { get; set; }
        public int Rating2 { get; set; }
        public int Rating3 { get; set; }
        public int Rating4 { get; set; }
        public int Rating5 { get; set; }
        public int Rating6 { get; set; }
        public int Rating7 { get; set; }
        public int Rating8 { get; set; }
        public long ServiceLookUp { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
        public int TotalRating { get; set; }
        public string UserDepartment { get; set; }
        public string UserLocation { get; set; }
        public string CategoryName { get; set; }
        public string SubCategory { get; set; }
        public string RequestType { get; set; }
        public string PRPUser { get; set; }
        public string OwnerUser { get; set; }
        public bool SLADisabled { get; set; }

        
    }
}
