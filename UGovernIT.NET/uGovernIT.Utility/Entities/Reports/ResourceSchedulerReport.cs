using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Reports
{
    public class ResourceSchedulerReport
    {
        public int id { get; set; }
        //public int parent { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string text { get; set; }
        public string TicketId { get; set; }
       // public string Project { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public int duration { get; set; }
        public string color { get; set; }
        public string ProjectStage { get; set; }
        public string ProjectManager { get; set; }
        public string APM { get; set; }
        public string Estimator { get; set; }
        public string Supritendent { get; set; }
        public string ProjectExecutive { get; set; }
    }
}
