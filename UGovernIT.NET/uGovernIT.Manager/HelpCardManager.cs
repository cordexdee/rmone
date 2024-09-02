using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class HelpCardManager : ManagerBase<HelpCard>, IHelpCardManagerManager
    {
        public HelpCardManager(ApplicationContext context) : base(context)
        {
            store = new HelpCardStore(this.dbContext);
        }
    }

    public interface IHelpCardManagerManager : IManagerBase<HelpCard>
    {

    }
}
