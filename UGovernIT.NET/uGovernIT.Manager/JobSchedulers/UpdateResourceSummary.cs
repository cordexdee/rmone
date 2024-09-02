using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Helpers;
using uGovernIT.Utility;

namespace uGovernIT.Manager.JobSchedulers
{
    public class UpdateResourceSummary : IJobScheduler
    {
        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext context = ApplicationContext.CreateContext(TenantID);
            ResourceSummary(context);
        }

        private void ResourceSummary(ApplicationContext context)
        {
            DataTable workItems = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            DataTable allocationItems = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocation, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            DataTable actualItems = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceTimeSheet, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            if (workItems != null && workItems.Rows.Count > 0)
            {
                RMMSummaryHelper.UpdateResourceUsageSummary(context, workItems, allocationItems, actualItems);
            }
        }
    }
}
