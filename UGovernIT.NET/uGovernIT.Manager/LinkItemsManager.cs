using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
   
    public class LinkItemManager : ManagerBase<LinkItems>, ILinkItemManager
    {
        public LinkItemManager(ApplicationContext context) : base(context)
        {
            store = new LinkItemStore(this.dbContext);
        }
        public LinkItems Save(LinkItems items)
        {
            if (items.ID > 0)
                this.Update(items);
            else
                this.Insert(items);
            return items;
        }
    }
    public interface ILinkItemManager : IManagerBase<LinkItems>
    {
        LinkItems Save(LinkItems item);
    }
}
