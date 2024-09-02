using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class ServiceSectionCondition
    {

        public Guid ID { get; set; }
   
        public string Title { get; set; }
     
        public string ConditionVar { get; set; }
    
        public string ConditionOperator { get; set; }
        
        public string ConditionVal { get; set; }
     
        public string ConditionValForWF { get; set; }
       
        public string Condition { get; set; }
       
        public List<WhereExpression> Conditions { get; set; }
       
        public List<long> SkipSectionsID { get; set; }
        
        public List<long> SkipQuestionsID { get; set; }
        
        public bool ConditionValidate { get; set; }

        public ServiceSectionCondition()
        {
            ID = Guid.NewGuid();
            Title = string.Empty;
            ConditionVal = string.Empty;
            ConditionVar = string.Empty;
            ConditionOperator = string.Empty;
            SkipSectionsID = new List<long>();
            SkipQuestionsID = new List<long>();
            Condition = string.Empty;

            Conditions = new List<WhereExpression>();
        }
    }
}
