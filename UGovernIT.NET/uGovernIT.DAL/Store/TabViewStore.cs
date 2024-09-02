using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
   public class TabViewStore:StoreBase<TabView>, ITabViewStore
    {
        public TabViewStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface ITabViewStore : IStore<TabView>
    {
    }
}
