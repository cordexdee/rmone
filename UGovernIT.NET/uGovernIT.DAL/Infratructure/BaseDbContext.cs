using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Utility.Entities.DMSDB;
using uGovernIT.Utility.Entities.DB.uGovernIT.Utility;
using uGovernIT.Util.Log;

namespace uGovernIT.DAL.Infratructure
{
    public class BaseDbContext : DbContext
    {
        protected CustomDbContext context;
        protected string _tenantID;

        public BaseDbContext(CustomDbContext context)
        {
            this.context = context;
            _tenantID = context.TenantID;
        }

        private void IncludeInAdd(object entity)
        {
            DBBaseEntity baseEntity = entity as DBBaseEntity;
            if (!UGITUtility.StringToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsMigrateddata"]))
            {
                if (baseEntity != null)
                {
                    baseEntity.Created = DateTime.Now;
                    baseEntity.Modified = DateTime.Now;
                    baseEntity.CreatedBy = Guid.Empty.ToString();
                    baseEntity.ModifiedBy = Guid.Empty.ToString();
                    if (context.CurrentUser != null)
                    {
                        baseEntity.CreatedBy = context.CurrentUser.Id;
                        baseEntity.ModifiedBy = context.CurrentUser.Id;
                    }
                }
            }
            if (baseEntity != null)
            {
                if (baseEntity.TenantID == null)
                    baseEntity.TenantID = context.TenantID;
                if (baseEntity.Created == null || baseEntity.Created == DateTime.MinValue)
                    baseEntity.Created = DateTime.Now;
                if (baseEntity.Modified == null || baseEntity.Modified == DateTime.MinValue)
                    baseEntity.Modified = DateTime.Now;
                if (baseEntity.CreatedBy == null)
                    baseEntity.CreatedBy = context.CurrentUser.Id;
                if (baseEntity.ModifiedBy == null)
                    baseEntity.ModifiedBy = context.CurrentUser.Id;
            }
        }

        private void IncludeInUpdate(object entity)
        {
            DBBaseEntity baseEntity = entity as DBBaseEntity;
            if (baseEntity != null)
            {
                if (baseEntity.Created == DateTime.MinValue)
                    baseEntity.Created = DateTime.Now;
                baseEntity.Modified = DateTime.Now;
                baseEntity.ModifiedBy = Guid.Empty.ToString();
                if (context.CurrentUser != null)
                {
                    if (string.IsNullOrWhiteSpace(baseEntity.CreatedBy))
                        baseEntity.CreatedBy = context.CurrentUser.Id;
                    baseEntity.ModifiedBy = context.CurrentUser.Id;
                }
                if (baseEntity.TenantID == null)
                {
                    baseEntity.TenantID = context.TenantID;
                }
            }
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            IncludeInAdd(entity);
            return base.Add(entity);
        }

        public override EntityEntry Add(object entity)
        {
            IncludeInAdd(entity);
            return base.Add(entity);
        }

        public override void AddRange(IEnumerable<object> entities)
        {
            foreach (object x in entities)
            {
                IncludeInAdd(x);
            }
            base.AddRange(entities);
        }

        public override void AddRange(params object[] entities)
        {
            foreach (object x in entities)
            {
                IncludeInAdd(x);
            }
            base.AddRange(entities);
        }

        public override Task<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            IncludeInAdd(entity);
            return base.AddAsync(entity, cancellationToken);
        }

        public override Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            IncludeInAdd(entity);
            return base.AddAsync(entity, cancellationToken);
        }

        public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (object x in entities)
            {
                IncludeInAdd(x);
            }
            return base.AddRangeAsync(entities, cancellationToken);
        }

        public override Task AddRangeAsync(params object[] entities)
        {
            foreach (object x in entities)
            {
                IncludeInAdd(x);
            }
            return base.AddRangeAsync(entities);
        }
      

        public override EntityEntry Update(object entity)
        {
            IncludeInUpdate(entity);
            return base.Update(entity);
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            IncludeInUpdate(entity);
            return base.Update(entity);
        }

        public override void UpdateRange(IEnumerable<object> entities)
        {
            foreach (object x in entities)
            {
                IncludeInUpdate(x);
            }
            base.UpdateRange(entities);
        }

        public override void UpdateRange(params object[] entities)
        {
            foreach (object x in entities)
            {
                IncludeInUpdate(x);
            }
            base.UpdateRange(entities);
        }

        public override int SaveChanges()
        {
            try
            {
                var entries = ChangeTracker.Entries().Where(e => e.Entity is DBBaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
                foreach (var entityEntry in entries)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        IncludeInAdd(entityEntry.Entity);
                    }
                    else
                    {
                        IncludeInUpdate(entityEntry.Entity);
                    }
                }
                return base.SaveChanges();
            }catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return -1;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.context.Database);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
