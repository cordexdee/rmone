using System;
using System.Linq;
using System.Xml;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
using System.Collections.Generic;
using uGovernIT.Util.Cache;
using Newtonsoft.Json;
using System.Data;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Util.Log;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Manager
{
    public interface IStatisticsConfigurationManager : IManagerBase<StatisticsConfiguration>
    {
        
    }

    public class StatisticsConfigurationManager : ManagerBase<StatisticsConfiguration>, IStatisticsConfigurationManager
    {
        public StatisticsConfigurationManager(ApplicationContext context) : base(context)
        {
            store = new StatisticsConfigurationStore(this.dbContext);
        }
    }
}
