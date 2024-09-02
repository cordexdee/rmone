using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
    public class OutputMapperPoint
    {
        public double XValue { get; set; }
        public double YValue { get; set; }
        public object Tooltip { get; set; }
        public string Label { get; set; }
        public string DateTimeValue { get; set; }
        public string YDateTimeValue { get; set; }
        public OutputMapperPoint(double xValue, double yValue)
        {
            XValue = xValue;
            YValue = yValue;
            Tooltip = string.Empty;
            Label = string.Empty;
            DateTimeValue = String.Empty;
            YDateTimeValue = string.Empty;
        }

        public OutputMapperPoint(double xValue, double yValue, string label, object tooltip,string dateTime, string yDatetime)
        {
            XValue = xValue;
            YValue = yValue;
            Tooltip = tooltip;
            Label = label;
            DateTimeValue = dateTime;
            YDateTimeValue = yDatetime;
            if (tooltip == null)
            {
                Tooltip = string.Empty;
            }
            if (label == null)
            {
                Label = string.Empty;
            }
            if (DateTimeValue == null)
            {
                DateTimeValue = string.Empty;
            }
            if (YDateTimeValue == null)
            {
                YDateTimeValue = string.Empty;
            }
        }

    }
}
