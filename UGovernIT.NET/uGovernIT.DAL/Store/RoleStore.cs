using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DAL.Store
{
    public class RoleStore<TRole> : StoreBase<TRole>, IRoleStore<TRole>, IQueryableRoleStore<TRole> where TRole : Role
    {
        private StoreBase<UserProfile> userStore;
        public RoleStore(CustomDbContext context) : base(context)
        {
           userStore = new DAL.StoreBase<UserProfile>(context);
        }

        public IQueryable<TRole> Roles
        {
            get
            {
                return this.Load().AsQueryable();
            }
        }

        public async Task CreateAsync(TRole role)
        {

            await Task.Run(() =>
            {
                this.Insert(role);
                UserProfile user = new UserProfile() { Id = role.Id, Name = role.Title, UserName = role.Name, isRole = true,Enabled=true, TenantID = role.TenantID, CreatedBy = role.CreatedBy };
                userStore.Insert(user);
            });
        }

        public async Task DeleteAsync(TRole role)
        {
            await Task.Run(() =>
            {
                this.Delete(role);
                UserProfile userProfile= userStore.Load(x => x.Id == role.Id).FirstOrDefault();
                if(userProfile!=null)
                userStore.Delete(userProfile);
            });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<TRole> FindByIdAsync(string roleId)
        {
            return await Task.Run(() =>
            {
                TRole role = this.Get(x=>x.Id == roleId);
                return role;
            });
        }

        public async Task<TRole> FindByNameAsync(string roleName)
        {
            return await Task.Run(() =>
            {
                TRole role = this.Get(x => x.Name == roleName && x.TenantID==this.context.TenantID);
                return role;
            });
        }

        public async Task UpdateAsync(TRole role)
        {
            this.Update(role);
            UserProfile userProfile = userStore.Load(x => x.Id == role.Id).FirstOrDefault();
            UserProfile user = new UserProfile() { Id = role.Id, Name = role.Title, UserName = role.Name, isRole = true, TenantID = role.TenantID, CreatedBy = role.CreatedBy };
            if (userProfile != null)
                userStore.Update(user);
            else
                userStore.Insert(user);

            await Task.FromResult(0);
        }
    }
}
