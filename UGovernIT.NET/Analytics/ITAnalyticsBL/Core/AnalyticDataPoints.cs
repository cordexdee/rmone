using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
    public class AnalyticDataPoints
    {
        public List<string> PointList { get; set; }
        public String DataType { get; set; }
        public String PointListHeader { get; set; }
        public long Id { get; set; }
        public ChartIntegration PointListSource { get; set; }

        public AnalyticDataPoints()
        {
            this.PointList = new List<string>();
            this.DataType = string.Empty;
            this.PointListHeader = string.Empty;
            this.PointListSource = new ChartIntegration();
        }
    }
}
