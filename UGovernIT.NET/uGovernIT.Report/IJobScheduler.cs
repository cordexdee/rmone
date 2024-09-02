using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Report
{
    public interface IReportJobScheduler
    {
       string Duration { get; }
       void Execute();
        
    }
}
