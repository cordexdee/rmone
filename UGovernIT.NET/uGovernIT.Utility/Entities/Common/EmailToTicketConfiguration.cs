using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
namespace uGovernIT.Utility
{
    public class EmailToTicketConfiguration
    {
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDelete { get; set; }
        public int PortNo { get; set; }
        public bool SSL { get; set; }
        public string ClientId { get; set; }
        public string TenantId { get; set; }
        public string SecretId { get; set; }

    }
}
