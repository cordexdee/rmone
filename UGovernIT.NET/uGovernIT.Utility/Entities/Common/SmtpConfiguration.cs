using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class SmtpConfiguration
    {
        public string SmtpFrom { get; set; }
        public int PortNo { get; set; }
        public bool SSL { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
