using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Helpers;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
namespace uGovernIT.Manager
{
   public class ResourceUsageSummaryWeekWiseManager : ManagerBase<ResourceUsageSummaryWeekWise>, IResourceUsageSummaryWeekWiseManager
    {
        public ResourceUsageSummaryWeekWiseManager(ApplicationContext context):base(context)
        {
            store = new ResourceUsageSummaryWeekWiseStore(this.dbContext);
        }
        public ResourceUsageSummaryWeekWise Save(ResourceUsageSummaryWeekWise resourceUsageSummaryWeekWise)
        {
            if (resourceUsageSummaryWeekWise.ID > 0 && this.LoadByID(resourceUsageSummaryWeekWise.ID) != null)
                this.Update(resourceUsageSummaryWeekWise);
            else
                this.Insert(resourceUsageSummaryWeekWise);
            return resourceUsageSummaryWeekWise;
        }

        /// <summary>
        /// Returns Open Tickets/Items related Allocations Summary only.
        /// </summary>   
        public List<ResourceUsageSummaryWeekWise> LoadOpenItems(ApplicationContext context, DateTime startDate, DateTime endDate, string filerOnUser = "")
        {
            TicketManager ticketManager = new TicketManager(context);
            DataTable dt = ticketManager.GetAllProjectTickets(false);
            List<string> ticketIds = dt.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)).ToList();
            var users = filerOnUser.Split(',').ToList();
            ResourceUsageSummaryWeekWiseManager resourceUsageSummaryWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
            List<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeekWises = resourceUsageSummaryWeekWiseManager.Load(x => !x.Deleted && x.Resource != "00000000-0000-0000-0000-000000000000" && x.ActualEndDate.Value >= startDate
                && x.ActualStartDate.Value <= endDate && (ticketIds.Contains(x.WorkItem) || x.WorkItemType == "Time Off") && users.Contains(x.Resource));
            return resourceUsageSummaryWeekWises?.ToList() ?? null;
             
        }

    }
    public interface IResourceUsageSummaryWeekWiseManager : IManagerBase<ResourceUsageSummaryWeekWise>
    {

    }
}
