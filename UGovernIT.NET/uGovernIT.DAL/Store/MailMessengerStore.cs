using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class MailMessengerStore : StoreBase<Email>, IMailMessengerStore
    {
        public MailMessengerStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface IMailMessengerStore : IStore<Email>
    { }
}
