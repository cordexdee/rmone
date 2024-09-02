using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
namespace uGovernIT.Manager
{
    public class VendorTypeManager:ManagerBase<VendorType>,IVendorTypeManager
    {
        public VendorTypeManager(ApplicationContext context) : base(context)
        {
            store = new VendorTypeStore(this.dbContext);
        }
    }
    public interface IVendorTypeManager : IManagerBase<VendorType>
    {

    }
}
