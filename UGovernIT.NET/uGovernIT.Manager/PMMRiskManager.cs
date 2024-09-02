using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class PMMRiskManager : ManagerBase<PMMRisks>, IPMMRiskManager
    {
        public PMMRiskManager(ApplicationContext context) : base(context)
        {
            store = new PMMRiskStore(this.dbContext);
        }
    }
    public interface IPMMRiskManager : IManagerBase<PMMRisks>
    {

    }
}
