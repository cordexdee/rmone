using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uGovernIT.Utility;
using uGovernIT.DAL;
using System.Threading.Tasks;
using System.Data;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager.JobSchedulers
{
    public class DailyTicketCounts : IJobScheduler
    {
        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext context = ApplicationContext.CreateContext(TenantID);
            UpdateDailyTicketCounts(context);
        }

        private void UpdateDailyTicketCounts(ApplicationContext context)
        {
            DateTime collectionDate = DateTime.Now.Date;
            ModuleViewManager mMgr = new ModuleViewManager(context);
            List<UGITModule> modules = mMgr.Load(x => x.EnableModule && x.KeepTicketCounts);
            ManagerBase<TicketCountTrends> tctMgr = new ManagerBase<TicketCountTrends>(context);

            foreach (UGITModule module in modules)
            {
                LifeCycle lifeCycle = null;
                LifeCycleStage resolvedStage = null;
                DataTable summaryData = uGITDAL.GetTable(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID} ='{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = '{module.ModuleName}'");
                if (summaryData == null)
                    continue;

                UGITModule moduleDetail = mMgr.GetByName(module.ModuleName);

                lifeCycle = moduleDetail.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                if (lifeCycle != null)
                    resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Resolved.ToString());


                int numCreated = 0;
                int numResolved = 0;
                int numClosed = 0;
                int totalActive = 0;
                int totalOnHold = 0;
                int totalResolved = 0;
                int totalClosed = 0;

                numCreated = summaryData.AsEnumerable().Where(x => UGITUtility.StringToDateTime(x[DatabaseObjects.Columns.TicketCreationDate]).Date == collectionDate).Count();
                numResolved = summaryData.AsEnumerable().Where(x => UGITUtility.StringToDateTime(x[DatabaseObjects.Columns.ResolvedDate]).Date == collectionDate).Count();
                numClosed = summaryData.AsEnumerable().Where(x => UGITUtility.StringToBoolean(x[DatabaseObjects.Columns.TicketClosed]) &&
                                                         UGITUtility.StringToDateTime(x[DatabaseObjects.Columns.ClosedDate]).Date == collectionDate).Count();

                totalOnHold = summaryData.AsEnumerable().Where(x => UGITUtility.StringToBoolean(x[DatabaseObjects.Columns.TicketOnHold])).Count();
                if (resolvedStage != null)
                    totalResolved = summaryData.AsEnumerable().Where(x => !UGITUtility.StringToBoolean(x[DatabaseObjects.Columns.TicketOnHold]) &&
                                                                 Convert.ToString(x[DatabaseObjects.Columns.ModuleStepLookup]).Equals(resolvedStage.ID)).Count();

                totalClosed = summaryData.AsEnumerable().Where(x => UGITUtility.StringToBoolean(x[DatabaseObjects.Columns.TicketClosed])).Count();
                totalActive = summaryData.Rows.Count - (totalResolved + totalOnHold + totalClosed);

                TicketCountTrends trendItem = tctMgr.Get(x => x.ModuleName == module.ModuleName && x.EndOfDay.HasValue && x.EndOfDay.Value.Date == collectionDate);

                if (trendItem == null)
                    trendItem = new TicketCountTrends();

                trendItem.ModuleName = module.ModuleName;
                trendItem.EndOfDay = collectionDate;
                trendItem.NumCreated = numCreated;
                trendItem.NumResolved = numResolved;
                trendItem.NumClosed = numClosed;
                trendItem.TotalActive = totalActive;
                trendItem.TotalClosed = totalClosed;
                trendItem.TotalOnHold = totalOnHold;
                trendItem.TotalResolved = totalResolved;
                if (trendItem.ID > 0)
                    tctMgr.Update(trendItem);
                else
                    tctMgr.Insert(trendItem);

            }

        }
    }
}
