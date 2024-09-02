using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
  public   class GaugeChartRange
    {
      public string RangeName { get; set; }
      public double Max { get; set; }
      public double Min { get; set; }
      public string RangeColor { get; set; }
      public string GaugeName { get; set; }

      public GaugeChartRange()
         {
             RangeName = string.Empty;
             RangeColor = string.Empty;
             Max = 0;
             Min = 0;
             GaugeName = string.Empty;
         }

    }
}
