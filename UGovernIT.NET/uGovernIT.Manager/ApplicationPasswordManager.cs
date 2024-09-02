using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.Common;

namespace uGovernIT.Manager
{
    public class ApplicationPasswordManager : ManagerBase<ApplicationPasswordEntity>, IApplicationPasswordManager
    {
        
        public ApplicationPasswordManager(ApplicationContext context):base(context)
        {
            
            store = new  ApplicationPasswordStore(this.dbContext);
        }
      
    }
    public interface IApplicationPasswordManager : IManagerBase<ApplicationPasswordEntity>
    {
       
    }
}
