using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
namespace uGovernIT.Manager
{
    public class ScheduleActionsArchiveManager: ManagerBase<SchedulerActionArchive>, IScheduleActionsArchiveManager
    {
        public ScheduleActionsArchiveManager(ApplicationContext context):base(context)
        {
            store = new ScheduleActionsArchiveStore(this.dbContext);
        }
        public void ScheduleActionArchiveCleanup()
        {
            string configurationdays = this.dbContext.ConfigManager.GetValue(ConfigConstants.ScheduleActionArchiveExpiration);

            int days = 90;
            if (!string.IsNullOrEmpty(configurationdays))
                days = int.Parse(configurationdays);
            DateTime currentDateTime = DateTime.Now;
            currentDateTime = currentDateTime.AddDays(-days);
            List<SchedulerActionArchive> listScheduleActionsArchive = Load(x => x.StartTime <= currentDateTime);
            if (listScheduleActionsArchive != null && listScheduleActionsArchive.Count > 0)
            {
                Delete(listScheduleActionsArchive);
            }
        }
    }
    public interface IScheduleActionsArchiveManager : IManagerBase<SchedulerActionArchive>
    {

    }
}
