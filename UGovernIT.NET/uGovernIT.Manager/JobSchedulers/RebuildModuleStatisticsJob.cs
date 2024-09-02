using DevExpress.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;


namespace uGovernIT.Manager.JobSchedulers
{
    public class RebuildModuleStatisticsJob : IJobScheduler
    {
        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext context = ApplicationContext.CreateContext(TenantID);
            ExecuteTask(context);
        }
        public void ExecuteTask(ApplicationContext context)
        {
            ModuleUserStatisticsManager ms = new ModuleUserStatisticsManager(context);
            ms.RebuildModuleUserStatistics(context);
        }
    }
}
