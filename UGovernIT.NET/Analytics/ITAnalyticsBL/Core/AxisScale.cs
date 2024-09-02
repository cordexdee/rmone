using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
   public  class AxisScale
    {
       public string Min;
       public string Max;
       public int Ticks;
       public string Format;
       public bool AutoScale;
       public int DecimalPrecision;
       public AxisScale()
       {
           this.Min = String.Empty;
           this.Max = String.Empty;
           this.Ticks = 0;
           this.DecimalPrecision = 0;
           this.Format = String.Empty;
           this.AutoScale = false; 
       }
    }
}
