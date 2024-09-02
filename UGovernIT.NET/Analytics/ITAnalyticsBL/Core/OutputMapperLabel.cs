using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
    public class OutputMapperLabel
    {
        public string LabelName { get; set; }
        public string Color { get; set; }
        public int Order { get; set; }
        public double Start { get; set; }
        public double End { get; set; }

        public OutputMapperLabel()
        {
            LabelName = string.Empty;
            Color = string.Empty;
        }
    }
}
