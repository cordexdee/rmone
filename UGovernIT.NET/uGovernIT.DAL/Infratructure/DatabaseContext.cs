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

namespace uGovernIT.DAL.Infratructure
{
    public class DatabaseContext : BaseDbContext
    {
        


        public DatabaseContext(CustomDbContext context):base(context)
        {
            

        }

        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<ConfigurationVariable> ConfigurationVariables { get; set; }
        public DbSet<Role> Role { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<ACRType> ACRTypes { get; set; }

        public DbSet<ApplicationAccess> ApplicationAccesses { get; set; }

        public DbSet<ApplicationModule> ApplicationModules { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationServer> ApplicationServers { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<AssetIncidentRelations> AssetIncidentRelations { get; set; }
        public DbSet<AssetModel> AssetModels { get; set; }

        public DbSet<AssetVendor> AssetVendors { get; set; }

        public DbSet<Assests> Assests { get; set; }

        public DbSet<BudgetActual> BudgetActuals { get; set; }

        public DbSet<BudgetCategory> BudgetCategories { get; set; }


        public DbSet<ChartFilter> ChartFilters { get; set; }

        public DbSet<ChartFormula> ChartFormulas { get; set; }

        public DbSet<ChartTemplate> ChartTemplates { get; set; }

        public DbSet<ClientAdminCategory> ClientAdminCategories { get; set; }

        public DbSet<ClientAdminConfigurationList> ClientAdminConfigurationLists { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyDivision> CompanyDivisions { get; set; }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<DashboardPanelView> DashboardPanelViews { get; set; }

        public DbSet<DashboardSummary> DashboardSummaries { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<DRQRapidType> DRQRapidTypes { get; set; }

        public DbSet<DRQSystemArea> DRQSystemAreas { get; set; }

        public DbSet<Email> Emails { get; set; }

        public DbSet<EventCategory> EventCategories { get; set; }

        public DbSet<FieldConfiguration> FieldConfigurations { get; set; }

        public DbSet<FunctionalArea> FunctionalAreas { get; set; }

        public DbSet<LifeCycle> LifeCycles { get; set; }
        public DbSet<LifeCycleStage> LifeCycleStages { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<MailTokenColumnName> MailTokenColumnNames { get; set; }
        public DbSet<MenuNavigation> MenuNavigations { get; set; }
        public DbSet<MessageBoard> MessageBoards { get; set; }
        public DbSet<ModuleColumn> ModuleColumns { get; set; }

        public DbSet<ModuleDefaultValue> ModuleDefaultValues { get; set; }
        public DbSet<ModuleEscalationRule> ModuleEscalationRules { get; set; }
        public DbSet<ModuleFormLayout> ModuleFormLayouts { get; set; }
        public DbSet<ModuleFormTab> ModuleFormTabs { get; set; }
        public DbSet<ModuleImpact> ModuleImpacts { get; set; }
        public DbSet<ModuleMonitorOption> ModuleMonitorOptions { get; set; }
        public DbSet<ModuleMonitorOptionHistory> ModuleMonitorOptionHistory { get; set; }
        public DbSet<ModuleMonitor> ModuleMonitors { get; set; }

        public DbSet<ModuleMonthlyBudget> ModuleMonthlyBudgets { get; set; }
        public DbSet<ModulePrioirty> ModulePrioirties { get; set; }
        // public DbSet<ModulePriorityMap> ModulePriorityMaps { get; set; }
        // public DbSet<ModuleRequestPriority> ModuleRequestPriorities { get; set; }
        public DbSet<ModulePriorityMap> ModulePriorityMaps { get; set; }
        public DbSet<ModuleRequestType> ModuleRequestTypes { get; set; }
        public DbSet<ModuleRequestTypeLocation> ModuleRequestTypeLocations { get; set; }

        public DbSet<ModuleRoleWriteAccess> ModuleRoleWriteAccesses { get; set; }

        public DbSet<UGITModule> UGITModules { get; set; }
        public DbSet<ModuleBudget> ModuleBudgets { get; set; }
        public DbSet<ModuleSeverity> ModuleSeverities { get; set; }
        public DbSet<ModuleSLARule> ModuleSLARules { get; set; }

        public DbSet<Module_StageType> ModuleStageTypes { get; set; }

        public DbSet<ModuleStatusMapping> ModuleStatusMappings { get; set; }
        public DbSet<ModuleTaskEmail> ModuleTaskEmails { get; set; }
        public DbSet<ModuleUserStatistic> ModuleUserStatistics { get; set; }
        public DbSet<ModuleUserType> ModuleUserTypes { get; set; }

        public DbSet<ModuleWorkflowHistory> ModuleWorkflowHistories { get; set; }

        public DbSet<NPRResource> NPRResources { get; set; }

        public DbSet<PageConfiguration> PageConfigurations { get; set; }

        public DbSet<ProjectClass> ProjectClasses { get; set; }
        public DbSet<ProjectInitiative> ProjectInitiatives { get; set; }

        public DbSet<SchedulerAction> ScheduleActions { get; set; }
        public DbSet<SchedulerActionArchive> ScheduleActionsArchives { get; set; }

        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<ServiceQuestion> ServiceQuestions { get; set; }
        public DbSet<ServiceQuestionMapping> ServiceQuestionMappings { get; set; }
        public DbSet<Services> Services { get; set; }

        public DbSet<ServiceSection> ServiceSections { get; set; }
        public DbSet<SubLocation> SubLocations { get; set; }

        //public DbSet<ServiceTask> ServiceTasks { get; set; }

        public DbSet<TabView> TabViews { get; set; }

        public DbSet<TaskTemplate> TaskTemplates { get; set; }

        public DbSet<ActualHour> TicketHours { get; set; }
        public DbSet<TicketRelation> TicketRelations { get; set; }
        //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
        public DbSet<TicketEvents> TicketEvents { get; set; }
        //
        public DbSet<TicketRelationship> TicketRelationships { get; set; }
        public DbSet<TicketTemplate> TicketTemplates { get; set; }

        public DbSet<UserLogin> UserLogins { get; set; }

        public DbSet<UserSkills> UserSkills { get; set; }
        public DbSet<UserCertificates> UserCertificates { get; set; }

        public DbSet<ExperiencedTag> ExperiencedTags { get; set; }
        public DbSet<WikiLeftNavigation> WikiLeftNavigations { get; set; }

        public DbSet<WorkflowSLASummary> WorkflowSLASummaries { get; set; }

        public DbSet<UGITTask> UGITTasks { get; set; }
        public DbSet<DashboardFactTables> FactTables { get; set; }
        public DbSet<GenericTicketStatus> GenericStatuses { get; set; }

        public DbSet<UGITEnvironment> UGITEnvironment { get; set; }
        public DbSet<ModuleStageConstraintTemplates> ModuleStageConstraintTemplates { get; set; }
        public DbSet<ModuleStageConstraints> ModuleStageConstraints { get; set; }
        //public DbSet<ServiceExtension> ServiceExtension { get; set; }
        public DbSet<SurveyFeedback> SurveyFeedback { get; set; }

        public DbSet<PMMComments> PMMComments { get; set; }
        public DbSet<PMMCommentHistory> PMMCommentHistory { get; set; }
        public DbSet<PMMIssues> PMMIssues { get; set; }
        public DbSet<ProjectMonitorState> ProjectMonitorState { get; set; }
        public DbSet<ProjectMonitorStateHistory> ProjectMonitorStateHistory { get; set; }
        public DbSet<PMMRisks> PMMRisks { get; set; }
        public DbSet<PMMEvents> PMMEvents { get; set; }
        public DbSet<BaseLineDetails> BaseLineDetails { get; set; }
        public DbSet<ModuleTasksHistory> ModuleTasksHistory { get; set; }
        public DbSet<ModuleBudgetHistory> ModuleBudgetHistory { get; set; }
        public DbSet<ModuleMonthlyBudgetHistory> ModuleMonthlyBudgetHistory { get; set; }
        public DbSet<ModuleBudgetsActualHistory> ModuleBudgetsActualHistory { get; set; }

        public DbSet<SprintSummary> SprintSummary { get; set; }
        public DbSet<Sprint> Sprint { get; set; }
        public DbSet<SprintTasks> SprintTasks { get; set; }
        public DbSet<ProjectReleases> ProjectReleases { get; set; }
        public DbSet<ModuleWorkflowHistoryArchive> ModuleWorkflowHistoryArchive { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ResourceAllocationMonthly> resourceAllocationMonthly { get; set; }
        public DbSet<ResourceWorkItems> resourceWorkItems { get; set; }

        public DbSet<ResourceTimeSheet> resourceTimeSheet { get; set; }
        public DbSet<SummaryResourceProjectComplexity> SummaryResourceProjectComplexity { get; set; }

        public DbSet<ResourceUsageSummaryMonthWise> resourceUsageSummaryMonthWise { get; set; }
        public DbSet<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeekWise { get; set; }
        public DbSet<RResourceAllocation> rresourceAllocation { get; set; }
        public DbSet<HolidaysAndWorkDaysCalendar> rHolidayAndWorkDaysCalander { get; set; }
        public DbSet<ApplicationPasswordEntity> applicationPasswordEntity { get; set; }
        public DbSet<ReportConfigData> ReportConfigData { get; set; }
        public DbSet<DecisionLog> DecisionLog { get; set; }
        public DbSet<DecisionLogHistory> DecisionLogHistory { get; set; }
        public DbSet<BusinessStrategy> BusinessStrategy { get; set; }
        public DbSet<LifeCycle> ModuleLifeCycles { get; set; }
        public DbSet<AnalyticDashboards> AnalyticDashboards { get; set; }
        public DbSet<GovernanceLinkCategory> GovernanceLinkCategory { get; set; }
        public DbSet<GovernanceLinkItem> GovernanceLinkItem { get; set; }
        public DbSet<ModuleFormLayout> ModuleFormLayout { get; set; }
        public DbSet<ModuleFormTab> ModuleFormTab { get; set; }
        public DbSet<LinkView> LinkViews { get; set; }
        public DbSet<LinkCategory> LinkCategories { get; set; }
        public DbSet<LinkItems> LinkItems { get; set; }
        public DbSet<VendorType> VendorType { get; set; }
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<TenantScheduler> TenantSchedulers { get; set; }
        public DbSet<WikiCategory> WikiCategory { get; set; }
        // public DbSet<UserRoles> UsersRoles { get; set; }
        public DbSet<LandingPages> LandingPages { get; set; }
        public DbSet<TenantRegistration> TenantRegistration { get; set; }

        //DMS
        public DbSet<DMSAccess> DMSAccess { get; set; }
        public DbSet<DMSAspnetApplications> Application { get; set; }
        //public DbSet<aspnet_Role> Role { get; set; }
        public DbSet<aspnet_User> User { get; set; }
        public DbSet<DMSCustomer> DMSCustomer { get; set; }
        public DbSet<DMSDirectory> DMSDirectories { get; set; }
        public DbSet<DMSDocument> DMSDocument { get; set; }
        public DbSet<DMSFileAuditLog> DMSFileAuditLog { get; set; }
        public DbSet<DMDocumentTypeList> DMDocumentTypeList { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        //public DbSet<aspnet_UsersInRole> UsersInRole { get; set; }
        public DbSet<DMSUsersFilesAuthorization> DMSUsersFilesAuthorization { get; set; }
        public DbSet<DMSTenantDocumentsDetails> DMSTenantDocumentsDetails { get; set; }
        public DbSet<ProjectStageHistory> ProjectStageHistory { get; set; }
        public DbSet<ProjectSimilarityConfig> ProjectSimilarityConfig { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<CRMRelationshipType> CRMRelationshipType { get; set; }
        public DbSet<CRMActivities> CRMActivities { get; set; }
        public DbSet<CheckListRoles> CheckListRoles { get; set; }
        public DbSet<CheckListTaskStatus> CheckListTaskStatus { get; set; }
        public DbSet<CheckListTasks> CheckListTasks { get; set; }
        public DbSet<CheckListTemplates> CheckListTemplates { get; set; }
        public DbSet<CheckLists> CheckLists { get; set; }
        public DbSet<CheckListTaskTemplates> CheckListTaskTemplates { get; set; }
        public DbSet<RelatedCompany> RelatedCompanies { get; set; }
        public DbSet<CheckListRoleTemplates> CheckListRoleTemplates { get; set; }

        public DbSet<ProjectAllocation> ProjectAllocation { get; set; }
        public DbSet<ProjectEstimatedAllocation> CRMProjectAllocation { get; set; }
        public DbSet<JobTitle> JobTitle { get; set; }
        public DbSet<ProjectAllocationTemplate> ProjectAllocationTemplates { get; set; }

        public DbSet<RankingCriterias> RankingCriterias { get; set; }
        public DbSet<LeadRanking> LeadRanking { get; set; }
        public DbSet<LeadCriteria> LeadCriteria { get; set; }
        public DbSet<ProjectComplexity> ProjectComplexity { get; set; }
        public DbSet<GlobalRole> GlobalRoles { get; set; }
        //HLP
        public DbSet<HelpCard> HelpCard { get; set; }
        public DbSet<HelpCardContent> HelpCardContent { get; set; }
        //wiki
        public DbSet<WikiArticles> WikiArticles { get; set; }
        public DbSet<WikiContents> WikiContents { get; set; }
        public DbSet<WikiDiscussion> WikiDiscussion { get; set; }
        public DbSet<WikiLinks> WikiLinks { get; set; }
        public DbSet<WikiReviews> WikiReviews { get; set; }
        public DbSet<WikiMenuLeftNavigation> WikiMenuLeftNavigation { get; set; }
        public DbSet<ReportMenu> ReportMenu { get; set; }
        public DbSet<ProjectStandardWorkItem> ProjectStandardWorkItems { get; set; }
        //Phrase
        public DbSet<Phrases> Phrases { get; set; }
        public DbSet<ServiceUpdates_Master> ServiceUpdates_Master { get; set; }

        //BackgroundProcessStatus
        public DbSet<BackgroundProcessStatus> BackgroundProcessStatus { get;set;}

        public DbSet<TicketCountTrends> TicketCountTrends { get; set; }
        public DbSet<EmployeeTypes> EmployeeTypes { get; set; }

        public DbSet<UGITLog> UGITLog { get; set; }
        public DbSet<Agents> Agents { get; set; }
        public DbSet<Studio> Studio { get; set; }
        public DbSet<ResourceTimeSheetSignOff> ResourceTimeSheetSignOff { get; set; }

        public DbSet<UserProjectExperience> UserProjectExperiences { get; set; }
    
        public DbSet<FunctionRole> FunctionRole { get; set; }
        public DbSet<FunctionRoleMapping> FunctionRoleMapping { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<StatisticsConfiguration> StatisticsConfiguration { get; set; }

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
                if (baseEntity.Created == null)
                    baseEntity.Created = DateTime.Now;
                if (baseEntity.Modified == null)
                    baseEntity.Modified = DateTime.Now;
                if (context.CurrentUser != null)
                {
                    if (baseEntity.CreatedBy == null)
                        baseEntity.CreatedBy = context.CurrentUser.Id;
                    if (baseEntity.ModifiedBy == null)
                        baseEntity.ModifiedBy = context.CurrentUser.Id;
                }
                else
                {
                    baseEntity.CreatedBy = Guid.Empty.ToString();
                    baseEntity.ModifiedBy = Guid.Empty.ToString();
                }
            }
        }

        private void IncludeInUpdate(object entity)
        {
            DBBaseEntity baseEntity = entity as DBBaseEntity;
            if (baseEntity != null)
            {
                baseEntity.Modified = DateTime.Now;
                baseEntity.ModifiedBy = Guid.Empty.ToString();
                if (context.CurrentUser != null)
                {
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


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.context.Database);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UGITModule>().HasKey(x => new { x.ModuleName, x.TenantID });
            modelBuilder.Entity<UGITModule>().HasAlternateKey(x => x.ID);
            modelBuilder.Entity<UGITModule>().Property(x => x.ID).UseSqlServerIdentityColumn().ValueGeneratedOnAdd();
            modelBuilder.Entity<UserLogin>().HasKey(x => new { x.LoginProvider, x.ProviderKey, x.UserId });
            modelBuilder.Entity<UserRole>().HasKey(x => new { x.RoleId, x.UserId });
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(x => typeof(DBBaseEntity).IsAssignableFrom(x.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType).Property(nameof(DBBaseEntity.Attachments)).HasDefaultValue("");
                modelBuilder.Entity(entityType.ClrType).Property(nameof(DBBaseEntity.TenantID)).HasDefaultValue("");
            }
            modelBuilder.Entity<WikiLeftNavigation>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<Appointment>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleImpact>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleSeverity>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<MenuNavigation>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleStageConstraints>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleSLARule>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<LifeCycle>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<Role>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<FieldConfiguration>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleDefaultValue>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleStageConstraintTemplates>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<PageConfiguration>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<UGITEnvironment>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<TicketTemplate>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ConfigurationVariable>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<Location>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<Department>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<Company>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<FunctionalArea>().HasQueryFilter(x => x.TenantID == _tenantID);
            //modelBuilder.Entity<LifeCycle>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleFormLayout>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleRoleWriteAccess>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleTaskEmail>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleRequestType>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModulePriorityMap>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleColumn>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleUserType>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModulePrioirty>().HasQueryFilter(x => x.TenantID == _tenantID);
            //modelBuilder.Entity<ModuleImpact>().HasQueryFilter(x => x.TenantID == _tenantID);
            //modelBuilder.Entity<ModuleSeverity>().HasQueryFilter(x => x.TenantID == _tenantID);
            //modelBuilder.Entity<ModuleRequestTypeLocation>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleFormTab>().HasQueryFilter(x => x.TenantID == _tenantID);

            //modelBuilder.Entity<MenuNavigation>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ACRType>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<DRQRapidType>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<DRQSystemArea>().HasQueryFilter(x => x.TenantID == _tenantID);

            modelBuilder.Entity<SchedulerAction>().HasQueryFilter(x => x.TenantID == _tenantID);
           
            modelBuilder.Entity<SurveyFeedback>().HasQueryFilter(x => x.TenantID == _tenantID);

            modelBuilder.Entity<Services>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ServiceCategory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ServiceQuestion>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ServiceQuestionMapping>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<UGITTask>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<UGITModule>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleUserStatistic>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<LifeCycleStage>().HasQueryFilter(x => x.TenantID == _tenantID);

            
            modelBuilder.Entity<TicketRelation>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<Email>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<MessageBoard>().HasQueryFilter(x => x.TenantID == _tenantID);            
            modelBuilder.Entity<WikiCategory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<DashboardPanelView>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ClientAdminConfigurationList>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ClientAdminCategory>().HasQueryFilter(x=> x.TenantID== _tenantID);
            modelBuilder.Entity<TabView>().HasQueryFilter(x => x.TenantID == _tenantID);

            modelBuilder.Entity<GenericTicketStatus>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleWorkflowHistory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<AssetModel>().HasQueryFilter(x => x.TenantID == _tenantID);
			modelBuilder.Entity<AssetVendor>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<BudgetCategory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<Dashboard>().HasQueryFilter(x => x.TenantID == _tenantID);

            modelBuilder.Entity<TaskTemplate>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectClass>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectInitiative>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<BusinessStrategy>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<UserSkills>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<UserCertificates>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ExperiencedTag>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<LinkView>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<LinkItems>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<LinkCategory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<GovernanceLinkItem>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<DashboardFactTables>().HasQueryFilter(x => x.TenantID == _tenantID);

            modelBuilder.Entity<SprintSummary>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<Sprint>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<SprintTasks>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectReleases>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectStageHistory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectSimilarityConfig>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectSimilarityMetrics>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<State>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CRMRelationshipType>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListRoles>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListTaskStatus>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListTasks>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListTemplates>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckLists>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListTaskTemplates>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<RelatedCompany>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListRoleTemplates>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<State>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CRMRelationshipType>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListRoles>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListTaskStatus>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListTasks>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListTemplates>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckLists>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListTaskTemplates>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<RelatedCompany>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CheckListRoleTemplates>().HasQueryFilter(x => x.TenantID == _tenantID);

            modelBuilder.Entity<RResourceAllocation>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ResourceAllocationMonthly>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ResourceWorkItems>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ResourceTimeSheet>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ResourceUsageSummaryMonthWise>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ResourceUsageSummaryWeekWise>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectAllocation>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<SummaryResourceProjectComplexity>().HasQueryFilter(x => x.TenantID == _tenantID);

            modelBuilder.Entity<JobTitle>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectAllocationTemplate>().HasQueryFilter(x => x.TenantID == _tenantID);

            modelBuilder.Entity<LeadRanking>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<RankingCriterias>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<LeadCriteria>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectEstimatedAllocation>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectComplexity>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<LandingPages>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<GlobalRole>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CompanyDivision>().HasQueryFilter(x => x.TenantID == _tenantID);            

            //wiki
            modelBuilder.Entity<WikiContents>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<WikiDiscussion>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<WikiLinks>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<WikiReviews>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<WikiArticles>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<WikiMenuLeftNavigation>().HasQueryFilter(x => x.TenantID == _tenantID);
          	modelBuilder.Entity<JobTitle>().HasQueryFilter(x => x.TenantID == _tenantID);

            //HelpCard
            modelBuilder.Entity<HelpCard>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<HelpCardContent>().HasQueryFilter(x => x.TenantID == _tenantID);

            //DMS
            modelBuilder.Entity<DMDocumentTypeList>().HasQueryFilter(x => x.TenantID == _tenantID);


            modelBuilder.Entity<LandingPages>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<GlobalRole>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<CompanyDivision>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<AssetModel>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<Phrases>().HasQueryFilter(x => x.TenantID == _tenantID);

            modelBuilder.Entity<ModuleBudget>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<BudgetActual>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleMonthlyBudget>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectMonitorState>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectMonitorStateHistory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleMonitor>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleMonitorOption>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleMonitorOptionHistory>().HasQueryFilter(x => x.TenantID == _tenantID);

            //PMM
            modelBuilder.Entity<BaseLineDetails>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleTasksHistory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleBudgetHistory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleMonthlyBudgetHistory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ModuleBudgetsActualHistory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ProjectStandardWorkItem>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<PMMComments>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<PMMCommentHistory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<DecisionLogHistory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<DecisionLog>().HasQueryFilter(x => x.TenantID == _tenantID);
            //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
            modelBuilder.Entity<TicketEvents>().HasKey(x => new { x.ID, x.Ticketid, x.ModuleName });
            modelBuilder.Entity<TicketEvents>().Property(x => x.ID).UseSqlServerIdentityColumn().ValueGeneratedOnAdd();
            
            modelBuilder.Entity<TicketCountTrends>().Property(x => x.ID).UseSqlServerIdentityColumn().ValueGeneratedOnAdd();
            modelBuilder.Entity<TicketCountTrends>().HasQueryFilter(x => x.TenantID == _tenantID);
            //BackgroundProcessStatus
            modelBuilder.Entity<BackgroundProcessStatus>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<TenantScheduler>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<VendorType>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ChartTemplate>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<EmployeeTypes>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<UGITLog>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ActualHour>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<DashboardSummary>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<Document>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ServiceSection>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<SubLocation>().HasQueryFilter(x => x.TenantID == _tenantID);

            modelBuilder.Entity<GovernanceLinkCategory>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<PMMEvents>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<MailTokenColumnName>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<SchedulerActionArchive>().HasQueryFilter(x => x.TenantID == _tenantID);
			modelBuilder.Entity<Agents>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<Studio>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<AssetIncidentRelations>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<ResourceTimeSheetSignOff>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<UserProjectExperience>().HasQueryFilter(x => x.TenantID == _tenantID);
            
            modelBuilder.Entity<FunctionRole>().HasQueryFilter(x => x.TenantID == _tenantID);
            modelBuilder.Entity<FunctionRoleMapping>().HasQueryFilter(x=>x.TenantID == _tenantID);
            modelBuilder.Entity<Statistics>();
            modelBuilder.Entity<StatisticsConfiguration>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
