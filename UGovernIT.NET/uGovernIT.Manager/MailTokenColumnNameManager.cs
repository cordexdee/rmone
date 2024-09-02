using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
namespace uGovernIT.Manager
{
    public class MailTokenColumnNameManager:ManagerBase<MailTokenColumnName>
    {
        public MailTokenColumnNameManager(ApplicationContext context):base(context)
        {
            store = new MailTokenColumnNameStore(this.dbContext);
        }
    }
    public interface IMailTokenColumnNameManager : IManagerBase<MailTokenColumnName>
    {

    }
}
