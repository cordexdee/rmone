using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
   
    public class AnalyticDashboardManager : ManagerBase<AnalyticDashboards>, IAnalyticDashboardManager
    {
        public AnalyticDashboardManager(ApplicationContext context) : base(context)
        {
            store = new AnalyticDashboardStore(this.dbContext);
        }
    }
    public interface IAnalyticDashboardManager : IManagerBase<AnalyticDashboards>
    {

    }
}
