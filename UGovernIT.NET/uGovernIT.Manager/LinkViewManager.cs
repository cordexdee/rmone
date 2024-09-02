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
   
    public class LinkViewManager : ManagerBase<LinkView>, ILinkViewManager
    {
        public LinkViewManager(ApplicationContext context) : base(context)
        {
            store = new LinkViewStore(this.dbContext);
        }        
        public LinkView Save(LinkView item)
        {
            if (item.ID > 0)
                this.Update(item);
            else
                this.Insert(item);
            return item;
        }
    }
    public interface ILinkViewManager : IManagerBase<LinkView>
    {
        LinkView Save(LinkView item);
    }
}
