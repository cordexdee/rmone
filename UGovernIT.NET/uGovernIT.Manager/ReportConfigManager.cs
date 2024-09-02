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
    public class ReportConfigManager:ManagerBase<ReportConfigData>, IReportConfigManager
    {
        public ReportConfigManager(ApplicationContext context) : base(context)
        {
            store = new ReportConfigStore(this.dbContext);
        }
        public ReportConfigData GetFilterDataByReportType(string fieldName)
        {
            ReportConfigData field = this.Get(x => x.ReportType == fieldName);
            return field;
        }
    }
    public interface IReportConfigManager : IManagerBase<ReportConfigData>
    {
        ReportConfigData GetFilterDataByReportType(string fieldName);
    }
}
