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
     public class SeverityManager: ManagerBase<ModuleSeverity>, ISeverityManager
    {
        public SeverityManager(ApplicationContext context): base(context)
        {
            store = new ModuleSeverityStore(this.dbContext);
        }
    }
    public interface ISeverityManager : IManagerBase<ModuleSeverity>
    {
       
    }
}
