using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
  public  class InterpretationExpression
    {
        public string SourceType { get; set; }
        public string Source { get; set; }
        public string SourceElement { get; set; }
        public int IntegrationID { get; set; }
        public string ElementDatatype { get; set; }

        public string PrefixOperator { get; set; }
        public string MiddleOperator { get; set; }

        public string ExpressionValue { get; set; }
      
        public string RightOpSourceType { get; set; }
        public string RightOpSource { get; set; }
        public string RightOpSourceElement { get; set; }
        public int RightOpIntegrationID { get; set; }
        public string RightOpElementDatatype { get; set; }

        public string RightOperandType { get; set; }

        public InterpretationExpression()
        {
            SourceType = string.Empty;
            Source = string.Empty;
            SourceElement = string.Empty;
            ElementDatatype = string.Empty;
            PrefixOperator = string.Empty;
            MiddleOperator = string.Empty;
            ExpressionValue = string.Empty;
            RightOpSourceType = string.Empty;
            RightOperandType = string.Empty;
            RightOpSourceElement = string.Empty;
            RightOpElementDatatype = string.Empty;
        }
       
    }
}
