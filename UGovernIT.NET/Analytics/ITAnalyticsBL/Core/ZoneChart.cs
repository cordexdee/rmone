using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
     public class ZoneChart : BubbleChart 
    {
         public ZoneChartArea ZoneData;

         public ZoneChart()
         {
             ZoneData = new ZoneChartArea();
         }
    }
}
