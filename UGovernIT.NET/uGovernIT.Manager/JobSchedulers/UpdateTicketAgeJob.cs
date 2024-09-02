using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager.JobSchedulers
{
    public class UpdateTicketAgeJob : IJobScheduler
    {
        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext context = ApplicationContext.CreateContext(TenantID);
            UpdateTicketAge(context);
        }

        private void UpdateTicketAge(ApplicationContext context)
        {
            ModuleViewManager mMgr = new ModuleViewManager(context);
            List<UGITModule> modules = mMgr.Load(x => x.EnableModule && !string.IsNullOrWhiteSpace(x.ModuleTable));
            ITicketManager ticketManager = new TicketManager(context);
            int workingHoursInADay = uHelper.GetWorkingHoursInADay(context, true);
            bool ticketAgeExcludesHoldTime = context.ConfigManager.GetValueAsBool(ConfigConstants.TicketAgeExcludesHoldTime);
            DataRow[] drColl = null;

            foreach (UGITModule module in modules)
            {
                try
                {
                    DataTable data = ticketManager.GetTickets(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

                    if (data == null || !uHelper.IfColumnExists(DatabaseObjects.Columns.TicketClosed, data))
                        continue;

                    drColl = data.Select(string.Format("{0} <> 'True' or Closed is null", DatabaseObjects.Columns.TicketClosed));
                    if (drColl == null || drColl.Length == 0)
                        continue;

                    data = drColl.CopyToDataTable();

                    if (!data.Columns.Contains(DatabaseObjects.Columns.TicketAge))
                    {
                        ULog.WriteLog(string.Format("Age column not exist in side   ...", data.Rows.Count, module.ModuleName));
                        continue;
                    }

                    ULog.WriteLog(string.Format("Updating ticket age for {0} {1} tickets ...", data.Rows.Count, module.ModuleName));

                    foreach (DataRow dataRow in data.Rows)
                    {
                        double ticketAge = Ticket.GetTicketAge(context, dataRow, workingHoursInADay, ticketAgeExcludesHoldTime);
                        dataRow[DatabaseObjects.Columns.TicketAge] = ticketAge;
                        ticketManager.Save(module, dataRow);
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteLog(string.Format("Age calculation for {0} :{1}", module.ModuleName, ex.Message));
                }

            }

            ULog.WriteLog("Updating ticket age job done!");
        }
    }
}
