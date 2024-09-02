using System.Collections.Generic;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
using uGovernIT.DAL;
using System;

namespace uGovernIT.Manager
{
    public interface ITenantManager : IManagerBase<Tenant>
    {

    }

    public class TenantManager : ManagerBase<Tenant>, ITenantManager
    {
        public TenantManager(ApplicationContext context) : base(context)
        {
            store = new TenantStore(this.dbContext);
        }

        public List<Tenant> GetTenantList()
        {
            return uGITDAL.GetTenantList();
        }

        public long UpdateItemCommon(Tenant obj)
        {
            return uGITDAL.UpdateItemCommon(obj);
        }

        public bool IsTrialTenant(string TenantId)
        {
            Tenant tenant = GetTenantById(TenantId);
            if (tenant != null && (tenant.Subscription == null || tenant.Subscription == 0 || tenant.Subscription == 1))
            {
                return true;
            }
            return false;
        }

       public bool IsDefaultTenant(string TenantId)
        {
            Tenant tenant = GetTenantById(TenantId);
            if (tenant != null && tenant.AccountID.Equals(ConfigHelper.DefaultTenant, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            return false;
            
        }


        //public List<UserProfile> GetTenantsUserList()
        //{
        //    return uGITDAL.GetTenantList();
        //}
    }
}
