using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace uGovernIT.Utility
{
    [Serializable]
    [XmlRoot("DashboardQuery")]
    public class DashboardQuery: DashboardPanel
    {
        public string Id { get; set; }
        public Guid QueryId { get; set; }
        public string QueryTable { get; set; }
        public String FactTable { get; set; }
        public string BasicFilter { get; set; }
        public CustomQuery QueryInfo { get; set; }
        public ScheduleAction ScheduleActionValue { get; set; }

        public DashboardQuery()
        {
            Id = string.Empty;
            QueryId = DashboardID;
            QueryTable = string.Empty;
            QueryInfo = new CustomQuery();
            ScheduleActionValue = new ScheduleAction();
        }
    }
}
