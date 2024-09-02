using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLite
{
    [Serializable]
    public class ConditionalFunction : Function
    {
        public List<Rule> Rules { get; set; }
        public string DefaultExpression { get; set; }
        public double MaxInputForDefault { get; set; }
        public double MinInputForDefault { get; set; }

        private string GetCorrectOperator(string op)
        {
            string correctOp = op;
            switch (op)
            {
                case "eq":
                    correctOp = "=";
                    break;
                case "neq":
                    correctOp = "!=";
                    break;
                case "lt":
                    correctOp = "<";
                    break;
                case "gt":
                    correctOp = ">";
                    break;
                case "gteq":
                    correctOp = ">=";
                    break;
                case "lteq":
                    correctOp = "<=";
                    break;
                default:
                    break;


                    //switch (op)
                    //{
                    //    case "eq":
                    //        op = "=";
                    //        break;
                    //    case "neq":
                    //        op = "!=";
                    //        break;
                    //    case "gt":
                    //        op = ">";
                    //        break;
                    //    case "lt":
                    //        op = "<";
                    //        break;
                    //    case "gteq":
                    //        op = ">=";
                    //        break;
                    //    case "lteq":
                    //        op = "<=";
                    //        break;

                    //    default:
                    //        break;
                    //}

            }
            return correctOp;
        }
        public override double Execute(FunctionInput function, bool isNormalizee, double normalizeBy)
        {

            double number = 0.0;
            if (function.Value != null)
            {
                double.TryParse(function.Value.ToString(), out number);
            }


            double absVal = 0.0;
            bool result = false;
            double val = 0.0;
            double normalizedVal = 0.0;
            double minNormalisedVal = 0.0;
            double maxNormalisedVal = 0.0;

        
            foreach (Rule rule in this.Rules)
            {
                rule.FirstConditionOp = GetCorrectOperator(rule.FirstConditionOp);

                //add code here to use conditions
                string firstExpression = String.Empty;
                if (rule.FirstConditionVal != "other")
                    firstExpression = rule.FirstCondition + " " + rule.FirstConditionOp + " " + rule.FirstConditionVal;
                else
                    firstExpression = rule.FirstCondition + " " + rule.FirstConditionOp + " " + rule.FirstConditionOtherVal;

                bool firstResult = ExpressionEvaluator.EvaluateCondition(firstExpression, function);
                result = firstResult;
                string secondExpression = String.Empty;
                if (rule.SecondCondition != String.Empty)
                {
                    rule.SecondConditionOp = GetCorrectOperator(rule.SecondConditionOp);
                    if (rule.SecondConditionVal != "other")
                        secondExpression = rule.SecondCondition + " " + rule.SecondConditionOp + " " + rule.SecondConditionVal;
                    else
                        secondExpression = rule.SecondCondition + " " + rule.SecondConditionOp + " " + rule.SecondConditionOtherVal;

                    bool secondResult = ExpressionEvaluator.EvaluateCondition(secondExpression, function);
                    result = firstResult && secondResult;
                }





                if (result == true && !string.IsNullOrWhiteSpace(rule.IfExpression))
                {
                    if (isNormalizee)
                    {
                        val = ExpressionEvaluator.Evaluate(rule.IfExpression, function);
                        if (rule.MaxInput == rule.MinInput)
                        {
                            ++rule.MaxInput;
                        }
                        normalizedVal = ((val - rule.MinInput) / (rule.MaxInput - rule.MinInput)) * normalizeBy;
                    }
                    else
                    {
                        absVal = ExpressionEvaluator.Evaluate(rule.IfExpression, function);
                    }
                    break;
                }
            }

            if (result == false && !string.IsNullOrWhiteSpace(this.DefaultExpression))
            {
                if (isNormalizee)
                {

                    val = ExpressionEvaluator.Evaluate(this.DefaultExpression, function, this.AskFrom, number);
                    if (this.MaxInputForDefault == this.MinInputForDefault)
                    {
                        ++this.MaxInputForDefault;
                    }
                    normalizedVal = ((val - this.MinInputForDefault) / (this.MaxInputForDefault - this.MinInputForDefault)) * normalizeBy;
                }
                else
                {
                    if (this.AskFrom == "dontAskFromUser")
                    {
                        absVal = ExpressionEvaluator.Evaluate(this.DefaultExpression, function);
                    }
                    else
                    {
                        absVal = ExpressionEvaluator.Evaluate(this.DefaultExpression, function, this.AskFrom, number);
                    }
                }
            }

            if (isNormalizee)
            {
                if (normalizedVal > maxNormalisedVal)
                    normalizedVal = maxNormalisedVal;
                if (normalizedVal < minNormalisedVal)
                    normalizedVal = minNormalisedVal;

                return normalizedVal;
            }
            else
                return
                    absVal;

        }
    }
}
