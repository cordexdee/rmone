using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace sLite
{
   [Serializable]
    public class NonLinearFunction : Function
    {
        
        public List<Rule> Rules { get; set; }
        public string DefaultExpression { get; set; }
        public double MaxInputForDefault { get; set; }
        public double MinInputForDefault { get; set; }
        public double ScaleFactor { get; set; }
        public override double Execute(FunctionInput function, bool isNormalize, double normalizeBy)
        {
            double number = 0.0;
            if (function.Value != null)
            {
                double.TryParse(function.Value.ToString(), out number);
            }
            

            
            bool result = false;
            double val = 0.0;
            double maxVal = 0.0;
            double normalizedVal = 0.0;
            double inputVal = number;
            //double startRange;
            //double endRange;
            int countMachExp = 0;
            //foreach (Rule rule in this.Rules)
            //{
            //    startRange = rule.StartRange;
            //    endRange = rule.EndRange;
            //    if (rule.StartRange > rule.EndRange)
            //    {
            //        startRange = rule.EndRange;
            //        //startRange = rule.StartRange;
            //    }

            //    if (rule.EndRange < inputVal && !string.IsNullOrWhiteSpace(rule.IfExpression))
            //    {
            //        result = true;
            //        countMachExp += 1;
            //        normalizedVal = val = ExpressionEvaluator.Evaluate(rule.IfExpression, rule.EndRange);
            //        if (isNormalize)
            //        {
            //            maxVal = ExpressionEvaluator.Evaluate(rule.IfExpression, rule.MaxInput);
            //            normalizedVal += val / maxVal;
            //        }
            //    }
            //    else
            //    {
            //        // Range should not be overlapped it should always be in order lik 0-5,0-10, 10-20
            //        if (rule.StartRange < inputVal && !string.IsNullOrWhiteSpace(rule.IfExpression))
            //        {
            //            countMachExp += 1;
            //            normalizedVal = val = ExpressionEvaluator.Evaluate(rule.IfExpression, number);
            //            if (isNormalize)
            //            {
            //                maxVal = ExpressionEvaluator.Evaluate(rule.IfExpression, rule.MaxInput);
            //                normalizedVal += val / maxVal;
            //            }
            //            break;
            //        }
            //    }
            //}

            if (result == false && !string.IsNullOrWhiteSpace(this.DefaultExpression))
            {
                //Apply Max input cap
                if (number > this.MaxInputForDefault)
                {
                    number = this.MaxInputForDefault;
                }

                if (function.functionValues != null && function.functionValues.Count > 0)
                {
                    FunctionValue fV = function.functionValues.FirstOrDefault(x => x.Key == this.DefaultExpression);
                    normalizedVal = val = ExpressionEvaluator.Evaluate(this.DefaultExpression, function);
                }
                else
                {
                    normalizedVal = val = ExpressionEvaluator.Evaluate(this.DefaultExpression, number);
                }              


                countMachExp = 1;
              
                if (isNormalize)
                {
                    maxVal = ExpressionEvaluator.Evaluate(this.DefaultExpression, this.MaxInputForDefault);
                    normalizedVal = val / maxVal;
                }

            }

            if (ScaleFactor == 0)
                ++ScaleFactor;

            return (normalizedVal * ScaleFactor)/countMachExp;
            
            
            // return base.Execute(value);
        }
    }

}
