using System;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.DAL.Infratructure;
using uGovernIT.DAL;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ITAnalyticsBL.DB
{
    public class ModelDB : BaseDbContext
    {
        public ModelDB() : base(CustomDbContext.Create(Convert.ToString(ConfigurationManager.ConnectionStrings["analyticdb"])))
        {

        }
        public ModelDB(CustomDbContext context) : base(context)
        {

        }
        public CustomDbContext GetDBContext()
        {
            return context;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DashboardModelInput>().HasKey(table => new { table.DashboardID, table.ModelInputID });
            modelBuilder.Entity<Model>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<ModelVersion>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<ModelInput>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<Dashboard>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<DashboardGroup>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<DashboardModelInput>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<ModelCategory>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<ModelOutputMapper>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<Interpretation>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<Question>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<ETTable>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<ETSchemaDraft>().HasQueryFilter(x => x.TenantID == this._tenantID);
            modelBuilder.Entity<ETSchemaInfo>().HasQueryFilter(x => x.TenantID == this._tenantID);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Model> Models { get; set; }      
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyCategory> SurveyCategories { get; set; }
        public DbSet<SurveyResult> SurveyResults { get; set; }
        public DbSet<SurveyRun> SurveyRuns { get; set; }
        public DbSet<SurveySection> SurveySections { get; set; }
        public DbSet<ModelVersion> ModelVersions { get; set; }
        public DbSet<ModelInput> ModelInputs { get; set; }
        public DbSet<Dashboard> AnalysisDashboards { get; set; }
        public DbSet<DashboardGroup> DashboardGroups { get; set; }
        public DbSet<DashboardModelInput> DashboardModelInputs { get; set; }
        public DbSet<ModelCategory> ModelCategories { get; set; }
        public DbSet<SideLink> SideLinks { get; set; }
        public DbSet<ModelOutputMapper> ModelOutputMappers { get; set; }
        public DbSet<Interpretation> Interpretations { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<ETTable> ETTables { get; set; }
        public DbSet<ETSchemaDraft> ETSchemaDrafts { get; set; }
        public DbSet<ETSchemaInfo> ETSchemaInfoes { get; set; }


        /// <summary>
        /// Get count of all archived models
        /// </summary>
        /// <returns>return count</returns>
        public int GetCountAllArchiveModels()
        {
            return (from p in this.Models
                    join
                    m in this.ModelVersions on p.ModelID equals m.ModelID
                    where m.Status == (int)AnalyticStatus.Archive
                    select m).Count();
        }

        /// <summary>
        /// Get count of all models except archived
        /// </summary>
        /// <returns>return count</returns>
        public int GetCountAllModelsExceptArchived()
        {
            return (from p in this.Models
                    join
                    m in this.ModelVersions on p.ModelID equals m.ModelID
                    where m.Status == (int)AnalyticStatus.Active
                    select m).Count();
        }

        /// <summary>
        /// Get count of all active models
        /// </summary>
        /// <returns>return count</returns>
        public int GetCountActiveModels()
        {
            return this.Models.Where(x => x.CurrentActiveVersionID > 0).Count();
        }

        public List<DashboardGroup> GetAllDashboardGroup()
        {
            return this.DashboardGroups.ToList();
        }


        public ModelVersion GetModelVersionDetail(Int64 modelVersionID)
        {
            return this.ModelVersions.Find(modelVersionID);
        }

       
    }

}

