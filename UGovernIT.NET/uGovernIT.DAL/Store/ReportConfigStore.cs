using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
   public class ReportConfigStore: StoreBase<ReportConfigData>, IReportConfigStore
    {
        public ReportConfigStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IReportConfigStore : IStore<ReportConfigData>
    {

    }
}
