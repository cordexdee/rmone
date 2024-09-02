using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager.JobSchedulers
{
    public class CleanupJobScheduler : IJobScheduler
    {
        //public string Duration
        //{
        //    get
        //    {
        //        return Cron.Monthly();
        //    }
        //}

        ApplicationContext _context = null;
        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            _context = ApplicationContext.CreateContext(TenantID);

            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_context);
            UGITLogManager logmanager = new UGITLogManager(_context);

            //get value of LogCleanDuration configuration variable
            string cleanDuration = configurationVariableManager.GetValue(ConfigConstants.LogCleanDuration);
            if (string.IsNullOrEmpty(cleanDuration))
                return;

            int logDuration = Math.Abs(UGITUtility.StringToInt(cleanDuration));
            if (logDuration == 0)
                return;

            //find the date, here we want to delete log files prior to this date
            DateTime logDate = DateTime.Now.AddDays(-logDuration);
            List<UGITLog> ugitLogList = logmanager.Load();
            if (ugitLogList == null || ugitLogList.Count == 0)
                return;

            //fetching out IDs from UGITLog list based on created date and deleting them using batch delete
            List<UGITLog> resultCollection = ugitLogList.Where(x => x.Created <= logDate).ToList();
            if (resultCollection != null && resultCollection.Count > 0)
            {
                ULog.WriteLog(string.Format("Deleting {0} entries from UGITLog that were created before {1}", resultCollection.Count, UGITUtility.GetDateStringInFormat(logDate, false)));
                //SPListHelper.DeleteBatch(spWeb, ugitLogList, resultCollection);
                logmanager.Delete(resultCollection);
            }
        }
    }
}
