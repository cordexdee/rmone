using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager.Managers
{
    public class ChartTemplatesManager : ManagerBase<ChartTemplate>
    {
        public ChartTemplatesManager(ApplicationContext context) : base(context)
        {
            store = new ChartTemplateStore(this.dbContext);
        }

        public ChartTemplate Save(ChartTemplate item)
        {
            if (item.ID > 0)
                this.Update(item);
            else
                this.Insert(item);
            return item;
        }
    }
    public interface IChartTemplatesManager : IManagerBase<ChartTemplate>
    {

    }
}
