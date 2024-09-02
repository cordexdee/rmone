using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager;

namespace uGovernIT.DxReport
{
   public interface IReportScheduler
    {
       string GetReport(ApplicationContext applicationContext, Dictionary<string, object> formobj, string attachFormat);
        Dictionary<string, object> GetDefaultData();
    }
}
