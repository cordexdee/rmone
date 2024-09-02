using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
   
    public class ServiceSectionInput
    {
        public bool IsSkiped { get; set; }
        public long SectionID { get; set; }
        public List<ServiceQuestionInput> Questions { get; set; }
        public ServiceSectionInput()
        {
            Questions = new List<ServiceQuestionInput>();
        }
    }
}
