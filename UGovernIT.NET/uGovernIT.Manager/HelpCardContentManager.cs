using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class HelpCardContentManager :ManagerBase<HelpCardContent>, IHelpCardContentManager
    {
        public HelpCardContentManager(ApplicationContext context) : base(context)
        {
            store = new HelpCardContentStore(this.dbContext);
        }

    }

    public interface IHelpCardContentManager : IManagerBase<HelpCardContent>
    {

    }
}
