using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class SPUserLookupValue
    {
        public int ID { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public SPUserLookupValue()
        {
            ID = 0;
            LoginName = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
        }
    }
}
