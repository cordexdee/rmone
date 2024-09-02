using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager
{
    public interface IJobScheduler
    {      
       string Duration { get; set; }
       Task Execute(string TenantID);
        
    }
}
