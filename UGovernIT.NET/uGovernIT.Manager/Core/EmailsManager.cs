using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class EmailsManager:ManagerBase<Email>
    {
        public EmailsManager(ApplicationContext context):base(context)
        {
            store = new EmailStore(this.dbContext);
        }
    }
    public interface IEmailsManager : IManagerBase<Email>
    {
    }
   
}
