using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class UserCertificateManager : ManagerBase<UserCertificates>, IUserCertificateManager
    {
        public UserCertificateManager(ApplicationContext context) : base(context)
        {
            store = new UserCertificateStore(this.dbContext);
        }
        public long Save(UserCertificates userCertificates)
        {
            if (userCertificates.ID > 0)
                this.Update(userCertificates);
            else
                this.Insert(userCertificates);
            return userCertificates.ID;
        }
    }
    public interface IUserCertificateManager : IManagerBase<UserCertificates>
    {

    }
}
