using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Serializable]
    public class ServiceQuestionInput
    {
        public bool IsSkiped { get; set; }
        public string Token { get; set; }
        public string Value { get; set; }
        public List<ServiceQuestionInput> SubTokensValue;
        public ServiceQuestionInput()
        {
            Token = string.Empty;
            Value = string.Empty;
            SubTokensValue = new List<ServiceQuestionInput>();
        }
    }
}
