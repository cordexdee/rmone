using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    // This Entity is used for Tenant to Tenant Data Migration.
    public class DMTenant
    {
        public string url { get; set; }
        public string dbconnection { get; set; }
        public string commondbconnection { get; set; }

        public string tenantid { get; set; }
        public string tenantname { get; set; }
        public string tenanturl { get; set; }

        public string TenantEmail { get; set; }
        public string contact { get; set; }
        public string title { get; set; }
        
        public string username { get; set; }
        public string password { get; set; }
        public string userEmail { get; set; }

        public string domain { get; set; }
        public string batchfetchsize { get; set; }
        public bool? SelfRegisteredTenant { get; set; }
    }
}
