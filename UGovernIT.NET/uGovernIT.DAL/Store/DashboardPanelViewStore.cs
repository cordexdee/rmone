using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class DashboardPanelViewStore : StoreBase<DashboardPanelView>, IDashboardPanelViewStore
    {
        public DashboardPanelViewStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IDashboardPanelViewStore : IStore<DashboardPanelView>
    {

    }
}
