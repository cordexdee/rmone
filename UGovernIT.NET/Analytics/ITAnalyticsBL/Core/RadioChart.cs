using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
  public   class RadioChart:AnalyticMapper
    {
        public List<RadioChartRule> RadioChartRules { get; set; }    
         public ChartIntegration RadioChartIntegration { get; set; }
         public List<RadioChartItem> RadioItemList { get; set; }

         public RadioChart()
         {
             this.RadioChartRules = new List<RadioChartRule>();
             this.RadioChartIntegration = new ChartIntegration();
             this.RadioItemList = new List<RadioChartItem>();
         }

    }
}
