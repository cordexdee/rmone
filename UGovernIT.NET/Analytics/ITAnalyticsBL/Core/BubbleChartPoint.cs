using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
   public class BubbleChartPoint : OutputMapperPoint
    {
        public string BubbleColor;
        public int BubbleFillType;
        public double BubbleRadius;
        public double BubbleValue;
        public BubbleChartPoint(double xValue, double yValue):base( xValue,  yValue)
        {
            this.XValue = XValue;
            this.YValue = YValue;
            this.BubbleColor = "#050505";        
            this.BubbleRadius = 5;
        }

        public BubbleChartPoint(double xValue, double yValue, string label, object tooltip, string bubbleColor, int fillType, double bubbleRadius, double bubbleVal, string dateTimeVal, string yDateTimeVal)
            : base(xValue, yValue, label, tooltip, dateTimeVal, yDateTimeVal)
        {
            XValue = xValue;
            YValue = yValue;
            Tooltip = tooltip;
            this.BubbleColor = bubbleColor;
            this.BubbleFillType = fillType;
            this.BubbleRadius = bubbleRadius;
            this.BubbleValue = bubbleVal;
            Label = label;
            if (tooltip == null)
            {
                Tooltip = string.Empty;
            }
            if (label == null)
            {
                Label = string.Empty;
            }
        }
    }
}
