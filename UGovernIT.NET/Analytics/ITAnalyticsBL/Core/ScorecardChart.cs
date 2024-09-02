using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
  public   class ScorecardChart:AnalyticMapper
    {
      public string Title { get; set; }
      public string Shape { get; set; }
      public string Size { get; set; }
      public string SatelliteShape { get; set; }
      public string SatelliteSize { get; set; }

      public string Background { get; set; }
      public string ForeColor { get; set; }
      public string SatelliteBackground { get; set; }
      public string SatelliteForeColor { get; set; }
      public string BackgroundImage { get; set; }
      public string KPIs { get; set; }

         public ScorecardChart()
         {
             Title = string.Empty;
             Shape = string.Empty;
             Size = string.Empty;
             SatelliteShape = string.Empty;
             SatelliteSize = string.Empty;
             Background = string.Empty;
             ForeColor = string.Empty;
             Background = string.Empty;
             Background = string.Empty;
             BackgroundImage = string.Empty;

             KPIs = string.Empty;
         }

    }
}
