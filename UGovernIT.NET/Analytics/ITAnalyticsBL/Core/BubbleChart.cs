using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
    public class BubbleChart : AnalyticMapper
    {
      
        public long ChartId;    
     
      
        public int LabelPosition;   
        public ChartIntegration BubbleSizeDataSource;          
        public List<BubbleColorRule> BubbleColorGroup;
        public BubbleAxis XAxis;
        public BubbleAxis YAxis;
        public double BubbleSizeMin;
        public double BubbleSizeMax;
        public int BubbleSizeUnit;
        public BubbleChart()
        {
            Title = String.Empty;
            ChartId = 0;
            ShowLabel = false;
            ModelVersionId = 0;
            LabelPosition = (int)ITAnalyticsBL.LabelPosition.Above;
            BubbleSizeDataSource = new ChartIntegration();
            BubbleColorGroup = new List<BubbleColorRule>();
            XAxis = new BubbleAxis();
            YAxis = new BubbleAxis();
        }
    }
}
