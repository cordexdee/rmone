using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
  public  class ZoneChartArea
    {
      public double StartTop { get; set; }
      public double StartBottom { get; set; }
      public double EndBottom { get; set; }
      public double EndTop { get; set; }
      public string ColorTop { get; set; }
      public string ColorBottom { get; set; }
      public string MiddleLineColor { get; set; }
      public string MiddleLineStyle { get; set; }
      public string MiddleLineWidth { get; set; }

      public ZoneChartArea()
      {
          ColorTop = String.Empty;
          ColorBottom = String.Empty;
          MiddleLineColor = string.Empty;
          MiddleLineStyle = string.Empty;
          MiddleLineWidth = string.Empty;


      }
    }
}
