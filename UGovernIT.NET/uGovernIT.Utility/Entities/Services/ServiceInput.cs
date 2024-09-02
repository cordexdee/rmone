using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
   
    public class ServiceInput
    {
        public long ServiceID { get; set; }
        public List<ServiceSectionInput> ServiceSections { get; set; }
        public ServiceInput()
        {
            ServiceSections = new List<ServiceSectionInput>();
        }
    }

}
