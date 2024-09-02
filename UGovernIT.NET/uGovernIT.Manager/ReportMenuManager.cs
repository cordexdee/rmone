using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class ReportMenuManager : ManagerBase<ReportMenu>, IReportMenuManager
    {
        public ReportMenuManager(ApplicationContext context) : base(context)
        {
            store = new ReportMenuStore(this.dbContext);
        }
    }
    public interface IReportMenuManager : IManagerBase<ReportMenu>
    {

    }
}
