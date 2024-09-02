using System;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class TenantRegistrationManager : ManagerBase<TenantRegistration>
    {
        public ApplicationContext _context = null;
        public TenantRegistrationManager(ApplicationContext context ) :base(context)
        {
            _context = context;
        }
    }
}
