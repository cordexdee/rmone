using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ChartTemplateStore : StoreBase<ChartTemplate>, IChartTemplateStore
    {
        public ChartTemplateStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface IChartTemplateStore : IStore<ChartTemplate>
    {

    }
}
