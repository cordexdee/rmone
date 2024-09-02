using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.Core
{
    public class MailTokenColumnNameManager: ManagerBase<Utility.MailTokenColumnName>, IMailTokenColumnNameManager
    {
        public MailTokenColumnNameManager(ApplicationContext context) : base(context)
        {

        }

    }
    public interface IMailTokenColumnNameManager : IManagerBase<Utility.MailTokenColumnName>
    {

    }
}
