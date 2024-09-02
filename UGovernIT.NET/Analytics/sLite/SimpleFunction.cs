using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLite
{
    [Serializable]
    public class SimpleFunction : Function
    {
        public string Expression { get; set; }
        public double MaxInput { get; set; }
        public double MinInput { get; set; }
        //public double AskFromValue { get; set; }
        public double ScaleFactor { get; set; }
        public override double Execute(FunctionInput functionInput, bool isNormalize, double normalizeBy)
        {
          //  bool test = ExpressionEvaluator.CheckCondition(this.Expression, Convert.ToDecimal(value));
            double number=0.0;         
            double normalizedVal = 0.0;
            double absoluteVal = 0.0;
           
            if (functionInput.Value != null)
            {
                double.TryParse(functionInput.Value.ToString(), out number);
            }
                     
            if (isNormalize)
            {
                double minNormalisedVal = 0.0;        //This is beacuse we are normalising between , 0 to 10, or 0 to 100 or 0 to 1000.
                if (this.MaxInput == this.MinInput)
                {
                    ++this.MaxInput;
                }
                double maxNormalisedVal = ((this.MaxInput - this.MinInput) / (this.MaxInput - this.MinInput)) * normalizeBy;

                double value = ExpressionEvaluator.Evaluate(this.Expression, functionInput);
                normalizedVal = ((value - this.MinInput) / (this.MaxInput - this.MinInput)) * normalizeBy;
                if (normalizedVal > maxNormalisedVal)
                    normalizedVal = maxNormalisedVal;
                if (normalizedVal < minNormalisedVal)
                    normalizedVal = minNormalisedVal;

            }
            else
            {
                absoluteVal = ExpressionEvaluator.Evaluate(this.Expression, functionInput);

            }

            if (ScaleFactor == 0)
                ++ScaleFactor;
           
            if (isNormalize)
            {
                return normalizedVal * ScaleFactor;
            }
            else
            {
                return absoluteVal * ScaleFactor;
            }
        }
    }
}
