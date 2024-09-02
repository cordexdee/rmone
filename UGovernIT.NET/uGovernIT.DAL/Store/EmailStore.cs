using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class EmailStore : StoreBase<Email>, IEmailStore
    {
        public EmailStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface IEmailStore:IStore<Email>
    {

    }

    
}
