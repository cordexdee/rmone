using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.DxReport
{
    public class WeeklyTeamPrfReportEntity
    {
        public DataTable Data { get; set; }
        public bool GroupByCategory { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
