using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
 public   class RadioChartItem
    {
     public string DisplayName { get; set; }
     public int ViewOrder { get; set; }
     public ChartIntegration DisplayItem { get; set; }
     public string Id { get; set; }
     public RadioChartItem()
     {
         this.DisplayItem = new ChartIntegration();
         this.DisplayName = string.Empty;
         this.Id = string.Empty;
     }
    }
}
