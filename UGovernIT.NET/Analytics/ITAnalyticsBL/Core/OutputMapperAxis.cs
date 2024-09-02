using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
  public  class OutputMapperAxis
    {
      public Guid RefId { get; set; }
      public string RefType { get; set; }
      public string Title { get; set; }
      public string AxisType { get; set; }
      public List<OutputMapperLabel> AxisLabelList { get; set; }
      public ChartIntegration AxisData { get; set; }
      public OutputMapperAxis()
      {
          AxisType = string.Empty;
          AxisLabelList = new List<OutputMapperLabel>();
          Title = string.Empty;
          RefType = string.Empty;
          RefId = Guid.Empty;
          AxisData = new ChartIntegration();

      }

      public OutputMapperAxis(string axisType): base()
      {
          AxisType = axisType;
          AxisData = new ChartIntegration();
      }
    }
}
