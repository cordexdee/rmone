using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
    public class RadioChartRule
    {
        public ChartIntegration RuleIntegration { get; set; }
        public String Threshold   { get; set; }
        public String Toggle { get; set; }
        public String FillType { get; set; }
        public String FillColor { get; set; }
        public int Activated { get; set; }
        public RadioChartRule()
        {
            RuleIntegration = new ChartIntegration();
            Threshold = string.Empty;
            Toggle = string.Empty;
            FillType = string.Empty;
            FillColor = string.Empty;
        }
    }
}
