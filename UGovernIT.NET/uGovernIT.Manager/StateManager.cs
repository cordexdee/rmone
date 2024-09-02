using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
namespace uGovernIT.Manager
{
    public class StateManager : ManagerBase<State>, IStateManager
    {
        public StateManager(ApplicationContext context) : base(context)
        {
            store = new StateStore(this.dbContext);
        }
    }
    public interface IStateManager : IManagerBase<State>
    {

    }
}