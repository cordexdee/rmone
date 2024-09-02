using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
   public  class BubblePair
    {
       public int PairWithAxis;
       public ChartIntegration PairData;

       public BubblePair()
       {
           PairData = new ChartIntegration();
           PairWithAxis = (int)ITAnalyticsBL.AxisType.XAxis;
       }
    }

}
