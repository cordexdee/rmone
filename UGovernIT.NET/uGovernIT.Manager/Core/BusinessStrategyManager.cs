using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;

namespace uGovernIT.Manager
{
    public class BusinessStrategyManager : ManagerBase<BusinessStrategy> , IBusinessStrategyManager
    {
        public BusinessStrategyManager(ApplicationContext context) : base(context)
        {

        }
    }
    public interface IBusinessStrategyManager : IManagerBase<BusinessStrategy>
    {

    }
}
