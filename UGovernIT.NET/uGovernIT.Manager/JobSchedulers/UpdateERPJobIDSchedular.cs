using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager.JobSchedulers
{
    public class UpdateERPJobIDSchedular : IJobScheduler
    {
        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext context = ApplicationContext.CreateContext(TenantID);
            UpdateERPJobID(context);
        }

        private static void UpdateERPJobID(ApplicationContext _context)
        {
            if (_context.TenantID != "")
            {
                try
                {
                    TicketManager ticketManager = new TicketManager(_context);
                    ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("@TenantID", _context.TenantID);
                    DataTable dt = GetTableDataManager.GetData("FillERPJOBIDNC", values);

                    List<string> lstttickets = new List<string>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow row = TicketManager.GetCurrentTicketbyId(_context, UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.TicketId]), ModuleNames.OPM);
                        ticketManager.UpdateTicketCache(row, moduleViewManager.GetByName(ModuleNames.OPM), false);
                        lstttickets.Add(UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.TicketId]));
                    }
                    if (lstttickets.Count > 0)
                    {
                        string message = string.Format("ERPJobId updated in Opportunities:{0}", string.Join(Constants.Separator6, lstttickets));
                        ULog.WriteUGITLog(_context.CurrentUser.Id, message, Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Module), _context.TenantID);
                    }
                }
                catch (Exception ex)
                {
                    string methodname = "UpdateERPJobIDSchedular";
                    ULog.WriteException(ex, $"Failed Method : {methodname}");
                }
                
            }
        }

    }

}
