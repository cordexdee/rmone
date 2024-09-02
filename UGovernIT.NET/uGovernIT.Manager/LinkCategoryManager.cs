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
   
    public class LinkCategoryManager : ManagerBase<LinkCategory>, ILinkCategoryManager
    {
        public LinkCategoryManager(ApplicationContext context) : base(context)
        {
            store = new LinkCategoryStore(this.dbContext);

        }
        public LinkCategory Save(LinkCategory item)
        {
            if (item.ID > 0)
                this.Update(item);
            else
                this.Insert(item);
            return item;
        }
    }
    public interface ILinkCategoryManager : IManagerBase<LinkCategory>
    {
        LinkCategory Save(LinkCategory item);
    }
}
