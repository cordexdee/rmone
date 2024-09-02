using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DAL.Store;


namespace uGovernIT.Manager
{
    public class InnerPageConfigurationManager : ManagerBase<InnerPageConfiguration>, IInnerPageConfigurationManager
    {
        public InnerPageConfigurationManager(ApplicationContext context): base(context)
        {
            store = new InnerPageConfigurationStore(this.dbContext);
        }
    }
    public interface IInnerPageConfigurationManager : IManagerBase<InnerPageConfiguration>
    {

    }
}
