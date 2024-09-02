using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLite
{
    [Serializable]
    public class MapFunction : Function
    {
        public List<Rule> Rules { get; set; }
        public string DefaultExpression { get; set; }
        public long MaxLimit { get; set; }
        public long MinLimit { get; set; }
        public double MaxInputForDefault { get; set; }
        public double MinInputForDefault { get; set; }

        public override double Execute(FunctionInput function, bool isNormalize, double normalizeBy)
        {
            return base.Execute(function, isNormalize, normalizeBy);
        }
    }
}
