using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
  
    [Table(DatabaseObjects.Tables.AnalyticDashboards)]
    public class AnalyticDashboards:DBBaseEntity
    {
        public long ID { get; set; }
        public int? AnalyticID { get; set; }
        public string AnalyticName { get; set; }
        public int? AnalyticVID { get; set; }
        public int? DashboardID { get; set; }
        public string Title { get; set; }
       
    }

}
