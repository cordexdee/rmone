using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using uGovernIT.Utility.Entities;
using System.Linq;
using System;

namespace uGovernIT.Manager
{
    internal class CustomUserValidator<T> : IIdentityValidator<UserProfile>
    {
        private UserProfileManager manager;

        public CustomUserValidator(UserProfileManager manager)
        {
            this.manager = manager;
        }

        public async Task<IdentityResult> ValidateAsync(UserProfile user)
        {
            bool isExistInMasterTenant = false;
            bool isExistInCurrentTenant = false;
            bool isCreaterMasterTenant = false;

            var errors = new List<string>();
            ApplicationContext applicationContext = ApplicationContext.Create();
            UserProfileManager userProfileManager = new UserProfileManager(applicationContext);
            UserProfile masterUser = userProfileManager.FindByName(user.UserName);

            if (applicationContext.TenantID.EqualsIgnoreCase(user.TenantID))
            {
                isCreaterMasterTenant = true;
            }

            if (masterUser != null)
            {
                isExistInMasterTenant = true;
            }

            if (!isCreaterMasterTenant)
            {
                var existingAccount = await manager.FindByNameAsync(user.UserName);
                if (existingAccount != null)
                {
                    isExistInCurrentTenant = true;
                }

                if (!isExistInMasterTenant)
                {
                    var UserAcrossTenant = userProfileManager.GetUserOnlyByUserName(user.UserName);
                    if (UserAcrossTenant != null)
                    {
                        // User name already in use across tenant
                        errors.Add("User name already exists.");
                    }
                }

                if (isExistInMasterTenant && isExistInCurrentTenant)
                {
                    // User name already in current tenant
                    errors.Add("User name already exists.");
                }
            }
            else
            {
                if (isExistInMasterTenant)
                {
                    // User name already in master tenant
                    errors.Add("User name already exists.");
                }
            }

            return errors.Any()
                    ? IdentityResult.Failed(errors.ToArray())
                    : IdentityResult.Success;
            throw new System.NotImplementedException();
        }
    }
}