using DevExpress.XtraEditors.Filtering.Templates;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Helpers;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
namespace uGovernIT.Manager
{
   public class ResourceAllocationMonthlyManager : ManagerBase<ResourceAllocationMonthly>, IResourceAllocationMonthlyManager
    {
        public ResourceAllocationMonthlyManager(ApplicationContext context):base(context)
        {
            store = new ResourceAllocationMonthlyStore(this.dbContext);
        }
        public ResourceAllocationMonthly Save(ResourceAllocationMonthly resourceAllocationMonthly)
        {
            if (resourceAllocationMonthly.ID > 0)
                this.Update(resourceAllocationMonthly);
            else
                this.Insert(resourceAllocationMonthly);
            return resourceAllocationMonthly;
        }

        /// <summary>
        /// Returns Open Tickets/Items related Allocations only.
        /// </summary>        
        public List<ResourceAllocationMonthly> LoadOpenItems(DateTime dateFrom, DateTime dateTo)
        {
            List<string> LstClosedTicketIds = new List<string>();       
            //get closed tickets instead of open tickets and then filter all except closet
            DataTable dtClosedTickets = RMMSummaryHelper.GetClosedTicketIds(this.dbContext);
            if (dtClosedTickets != null && dtClosedTickets.Rows.Count > 0)
            {
                LstClosedTicketIds.AddRange(dtClosedTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
            }

            List<ResourceAllocationMonthly> resourceAllocationMonthlies = new List<ResourceAllocationMonthly>();
            resourceAllocationMonthlies = this.Load(x => x.MonthStartDate.Value.Date >= Convert.ToDateTime(dateFrom).Date && x.MonthStartDate.Value.Date <= Convert.ToDateTime(dateTo) && !LstClosedTicketIds.Any(y => x.ResourceWorkItem == y));
            return resourceAllocationMonthlies;
        }


        /// <summary>
        /// Returns Open Tickets/Items related Allocations only.
        /// </summary>        
        public List<ResourceAllocationMonthly> LoadOpenItems(string query)
        {
            List<string> LstClosedTicketIds = new List<string>();
            //get closed ticket ids instead of opentickets and then filter all except closed tickets
            DataTable dtClosedTickets = RMMSummaryHelper.GetClosedTicketIds(this.dbContext);
            if (dtClosedTickets != null && dtClosedTickets.Rows.Count > 0)
            {
                LstClosedTicketIds.AddRange(dtClosedTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
            }

            List<ResourceAllocationMonthly> resourceAllocationMonthlies = new List<ResourceAllocationMonthly>();
            resourceAllocationMonthlies = this.Load(query);
            resourceAllocationMonthlies = resourceAllocationMonthlies.Where(x => !LstClosedTicketIds.Any(y => x.ResourceWorkItem == y)).ToList();
            
            return resourceAllocationMonthlies;
        }

        public DataTable LoadAllocationMonthlyView(DateTime dateFrom, DateTime dateTo, bool IncludeClosed)
        {
            try
            {
                string ModuleNames = "CPR,OPM,CNS,PMM,NPR";
                DataTable dtAllocationMonthWise = null;
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", this.dbContext.TenantID);
                values.Add("@ModuleNames", ModuleNames);
                values.Add("@Fromdate", dateFrom);
                values.Add("@Todate", dateTo);
                values.Add("@Isclosed", IncludeClosed);
                string keyAllocationMonthly = "dtAllocationMonthly_" + dateFrom.ToLongDateString() + "_" + dateTo.ToLongDateString();

                bool keyStatus = false; // CacheHelper<object>.IsExists(keyAllocationMonthly, this.dbContext.TenantID);
                if (keyStatus)
                    dtAllocationMonthWise = ((DataTable)CacheHelper<object>.Get(keyAllocationMonthly, this.dbContext.TenantID)); //CacheHelper<DataTable>.Get(keyAllocationMonthly, context.TenantID);
                else
                {
                    dtAllocationMonthWise = uGITDAL.ExecuteDataSetWithParameters("usp_GetAllocationdata", values);
                    CacheHelper<object>.AddOrUpdate(keyAllocationMonthly, this.dbContext.TenantID, dtAllocationMonthWise);
                }

                return dtAllocationMonthWise;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return null;
        }
    }
    public interface IResourceAllocationMonthlyManager : IManagerBase<ResourceAllocationMonthly>
    {

    }
}
