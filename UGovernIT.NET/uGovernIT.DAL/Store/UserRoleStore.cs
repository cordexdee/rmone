using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using uGovernIT.Utility.Entities;
using System.Data.SqlClient;

namespace uGovernIT.DAL.Store
{
    public class UserRoleStore<TRole> : StoreBase<TRole>, IRoleStore<TRole>, IQueryableRoleStore<TRole> where TRole : UserRoles
    {
        public UserRoleStore(CustomDbContext context):base(context)
        {

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
            });
        }

        public async Task DeleteAsync(TRole role)
        {
            await Task.Run(() =>
            {
                this.Delete(role);
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
            await Task.FromResult(0);
        }
    }
}
