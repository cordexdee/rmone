using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DAL.Store
{
    public class ReportMenuStore : StoreBase<ReportMenu>, IReportMenuStore
    {
        public ReportMenuStore(CustomDbContext context) : base(context)
        {

        }             

    }

    public interface IReportMenuStore : IStore<ReportMenu>
    {

    }
}
