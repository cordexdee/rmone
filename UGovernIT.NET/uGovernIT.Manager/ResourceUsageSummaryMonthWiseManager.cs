using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
namespace uGovernIT.Manager
{
   public class ResourceUsageSummaryMonthWiseManager : ManagerBase<ResourceUsageSummaryMonthWise>, IResourceUsageSummaryMonthWiseManager
    {
        public ResourceUsageSummaryMonthWiseManager(ApplicationContext context):base(context)
        {
            store = new ResourceUsageSummaryMonthWiseStore(this.dbContext);
        }
        public ResourceUsageSummaryMonthWise Save(ResourceUsageSummaryMonthWise resourceUsageSummaryMonthWise)
        {
            if (resourceUsageSummaryMonthWise.ID > 0 && this.LoadByID(resourceUsageSummaryMonthWise.ID) != null)
                this.Update(resourceUsageSummaryMonthWise);
            else
                this.Insert(resourceUsageSummaryMonthWise);
            return resourceUsageSummaryMonthWise;
        }

    }
    public interface IResourceUsageSummaryMonthWiseManager : IManagerBase<ResourceUsageSummaryMonthWise>
    {

    }
}
