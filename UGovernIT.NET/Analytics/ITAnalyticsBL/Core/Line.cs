using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
   public class Line
    {
       public ChartIntegration LineData { get; set; }
       public string AxisReference { get; set; }
       public string LineColor { get; set; }
       public string LineSize { get; set; }
       public string LineStyle { get; set; }
       public string LineLabel { get; set; }
       public double Max { get; set; }
       public double Min { get; set; }
       public bool IsTrendLineRequired { get; set; }
       public bool IsTrendLine { get; set; }
       public Line()
       {
           LineData = new ChartIntegration();
           AxisReference = String.Empty;
           LineColor = String.Empty;
           LineSize = String.Empty;
           LineStyle = String.Empty;
           LineLabel = String.Empty;
           
       }
       public Line(Guid refId, string reftype, long analyticVersionId, string dataType, string axisRef, string lineCol, string lineSize, string lineStyle, string label, double max, double min, string nodeName  , bool isTrend)
           : this()
       {
          
           LineData.RefId = refId;
           LineData.RefType = reftype;
           LineData.ModelVersionId = analyticVersionId;
           LineData.DataType  =dataType;
           LineData.NodeName = nodeName;
           AxisReference = axisRef;
           LineColor = lineCol;
           LineSize = lineSize;
            LineStyle = lineStyle;
            LineLabel = label;
            Max = max;
            Min = min;
            IsTrendLineRequired = isTrend;
       }
    }
}
