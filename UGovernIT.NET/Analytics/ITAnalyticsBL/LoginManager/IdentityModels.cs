using ITAnalyticsBL.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ITAnalyticsBL.Extension;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility;
using Microsoft.EntityFrameworkCore;

namespace ITAnalyticsBL.LoginManager
{
    public class ApplicationDbContext : DbContext
    {
        public long TenantID { get; private set; }
        public bool IsRootUser { get; private set; }

        public ApplicationDbContext() : base()
        {
            this.IsRootUser = true;
            if (HttpContext.Current.GetCurrentTenantId().HasValue)
            {
                this.TenantID = HttpContext.Current.GetCurrentTenantId().Value;
                this.IsRootUser = false;
            }
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Filter("TenantFilter", (DBBaseEntity d) => d.TenantID, (ApplicationDbContext ctx) => ctx.TenantID);
            //modelBuilder.EnableFilter("TenantFilter", (ApplicationDbContext ctx) => !ctx.IsRootUser);
            
            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges()
        {
            long companyId = Convert.ToInt64(HttpContext.Current.GetCurrentTenantId());
            if (companyId > 0)
            {
                var addedEntities = this.ChangeTracker.Entries().Where(c => c.State == EntityState.Added)
                    .Select(c => c.Entity).OfType<DBBaseEntity>();

                foreach (var entity in addedEntities)
                {
                    if (!string.IsNullOrWhiteSpace(entity.TenantID))
                        entity.TenantID = entity.TenantID;
                    else
                        entity.TenantID = Guid.Empty.ToString();
                }
            }
            return base.SaveChanges();
        }
        //protected override DbEntityValidationResult ValidateEntity( DbEntityEntry entityEntry, IDictionary<object, object> items)
        //{
        //    if (entityEntry != null && entityEntry.State == EntityState.Added)
        //    {
        //        var errors = new List<DbValidationError>();
        //        var user = entityEntry.Entity as User;

        //        if (user != null)
        //        {
        //            if (this.Users.Any(u => string.Equals(u.UserName, user.UserName)
        //              && u.CompanyID == user.CompanyID))
        //            {
        //                errors.Add(new DbValidationError("User",
        //                  string.Format("Username {0} is already taken for AppId {1}",
        //                    user.UserName, user.CompanyID)));
        //            }

        //            if (this.RequireUniqueEmail
        //              && this.Users.Any(u => string.Equals(u.Email, user.Email)
        //              && u.CompanyID == user.CompanyID))
        //            {
        //                errors.Add(new DbValidationError("User",
        //                  string.Format("Email Address {0} is already taken for AppId {1}",
        //                    user.UserName, user.CompanyID)));
        //            }                   
        //        }
        //        else
        //        {
        //            var role = entityEntry.Entity as Role;
        //            if (role != null && this.roles.Any(r => string.Equals(r.Name, role.Name) && r.CompanyID==role.CompanyID))
        //            {
        //                errors.Add(new DbValidationError("Role",
        //                  string.Format("Role {0} already exists", role.Name)));
        //            }
        //        }
        //        if (errors.Any())
        //        {
        //            return new DbEntityValidationResult(entityEntry, errors);
        //        }
        //    }
        //    return new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
        //}
    }
}
