using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
  public   class GaugeChart:AnalyticMapper
    {
      public string Title { get; set; }
      public string Shape { get; set; }
      public string Style { get; set; }
      public string KPIShape { get; set; }
      public string KPIStyle { get; set; }
      public List<GaugeChartRange> GaugeRange { get; set; }
      public string Background { get; set; }
      public string ForeColor { get; set; }
      public string SatelliteBackground { get; set; }
      public string SatelliteForeColor { get; set; }
      public string BackgroundImage { get; set; }
      public string KPIs { get; set; }
      public List<GaugeChartRange> KPIRange { get; set; }
      public GaugeChart()
         {
             Title = string.Empty;
             Shape = string.Empty;
             this.Style = string.Empty;
             KPIShape = string.Empty;
             KPIStyle = string.Empty;
             Background = string.Empty;
             ForeColor = string.Empty;
             Background = string.Empty;
             Background = string.Empty;
             BackgroundImage = string.Empty;

             KPIs = string.Empty;
         }

    }
}
