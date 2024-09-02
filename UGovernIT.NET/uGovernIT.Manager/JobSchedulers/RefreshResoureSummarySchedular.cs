using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uGovernIT.DAL;
using uGovernIT.Helpers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager.JobSchedulers
{
    public class RefreshResoureSummarySchedular : IJobScheduler
    {
        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext context = ApplicationContext.CreateContext(TenantID);
            FillResourceSummaryUtilization(context);
        }
        private void FillResourceSummaryUtilization(ApplicationContext _context)
        {
            try
            {
                if (_context.TenantID != "")
                {
                    bool status = RMMSummaryHelper.FillResourceUtilization(_context);
                    if (!status)
                        ULog.WriteLog(string.Format("ERROR in FillResourceUtilization Job"));
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex.ToString());
            }

        }
    }
}
