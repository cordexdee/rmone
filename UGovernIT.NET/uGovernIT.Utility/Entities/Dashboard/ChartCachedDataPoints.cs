using System;
using System.Data;

namespace uGovernIT
{
    public class ChartCachedDataPoints
    {
        public string ChartID { get; set; }
        public System.Web.UI.DataVisualization.Charting.SeriesCollection ChartSeries { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Title { get; set; }
        public bool IsUpdating { get; set; }
        public DataTable DataPoints { get; set; }
    }
}
