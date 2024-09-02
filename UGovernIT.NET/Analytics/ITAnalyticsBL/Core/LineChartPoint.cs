using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
   public class LineChartPoint
    {
       public List<OutputMapperPoint> PointsList { get; set; }
       public string LineColor { get; set; }
       public string LineStyle { get; set; }
       public string AxisReference { get; set; }
       public string LineSize { get; set; }
       public LineChartPoint()
       {
           PointsList = new List<OutputMapperPoint>();
           LineColor = String.Empty;
           LineStyle = String.Empty;
           AxisReference = String.Empty;
           LineSize = String.Empty;
       }
      
    }
}
