using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
   public class LineChart:AnalyticMapper
    {
        
        public List<Line> LineGroup { get; set; }
        public BubbleAxis XAxis { get; set; }
        public BubbleAxis YAxis { get; set; }
        public BubbleAxis AltYAxis { get; set; }
        public bool HideAltAxis { get; set; }
      
        public LineChart()
        {
            LineGroup = new List<Line>();
            XAxis = new BubbleAxis();
            YAxis = new BubbleAxis();
            AltYAxis = new BubbleAxis();
            HideAltAxis = false;
           
        }
    }
}
