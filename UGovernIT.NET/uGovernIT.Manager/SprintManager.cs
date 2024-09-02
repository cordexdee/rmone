using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class SprintManager : ManagerBase<Sprint>, ISprintManager
    {
        public SprintManager(ApplicationContext context) : base(context)
        {
            store = new SprintStore(this.dbContext);
        }
    }
    public interface ISprintManager : IManagerBase<Sprint>
    {

    }
}
