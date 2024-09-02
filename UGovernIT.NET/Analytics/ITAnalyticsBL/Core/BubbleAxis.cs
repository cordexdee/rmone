using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ITAnalyticsBL.Core
{
  public class BubbleAxis
    {
      public int AxisType{ get; set; }
      public OutputMapperLabel AxisLabel { get; set; }
      public ChartIntegration AxisData{ get; set; }
      public AxisScale Scale{ get; set; }

      public BubbleAxis()
      {
          this.AxisType = (int)ITAnalyticsBL.AxisType.XAxis;
          AxisLabel = new OutputMapperLabel();
          this.AxisData = new ChartIntegration();
          Scale = new AxisScale();
      }
    }


}
