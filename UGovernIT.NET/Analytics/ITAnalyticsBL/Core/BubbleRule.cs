using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
   public class BubbleColorRule
    {
       public string Criteria;
       public double Max;
       public double Min;
       public string Color;
       public int Filltype;

       public BubbleColorRule()
       {
           Criteria = string.Empty;
           Color = "blue";
       }
       public BubbleColorRule(string criteria,double max, double min,string color, int fillType)
       
       {
           this.Criteria = criteria;
           this.Max = max;
           this.Min = min;
           this.Color = color;
           this.Filltype = fillType;

       }

    }
}
