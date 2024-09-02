using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class UserCertificateStore : StoreBase<UserCertificates>, IUserCertificateStore
    {
        public UserCertificateStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IUserCertificateStore : IStore<UserCertificates>
    {
    }
}
