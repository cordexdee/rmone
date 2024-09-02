using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class VendorSOWInvoiceDetailManager : ManagerBase<VendorSOWInvoiceDetail>, IVendorSOWInvoiceDetailManager
    {
        public VendorSOWInvoiceDetailManager(ApplicationContext context) : base(context)
        {
            store = new VendorSOWInvoiceDetailStore(this.dbContext);
        }
    }
    public interface IVendorSOWInvoiceDetailManager : IManagerBase<VendorSOWInvoiceDetail>
    {

    }
}
