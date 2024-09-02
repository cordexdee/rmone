using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
    public class PairWiseChart:BubbleChart
    {
        public BubblePair PairData;
        public BubbleColorRule BubbleColor;
        public BubbleColorRule PairColor;
        public PairWiseChart()
        {
            PairData = new BubblePair();
            BubbleColor = new BubbleColorRule();
            PairColor = new BubbleColorRule();
        }
    }
}
