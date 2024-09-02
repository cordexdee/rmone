using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class SprintTaskManager : ManagerBase<SprintTasks>, ISprintTaskManager
    {
        public SprintTaskManager(ApplicationContext context) : base(context)
        {
            store = new SprintTaskStore(this.dbContext);
        }
    }
    public interface ISprintTaskManager : IManagerBase<SprintTasks>
    {

    }
}
