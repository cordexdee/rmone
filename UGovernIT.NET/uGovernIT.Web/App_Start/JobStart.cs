using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.Server;
using Hangfire.SqlServer;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.Common;


namespace uGovernIT.Web
{
    public class JobStart
    {
        public static void Start(IAppBuilder app)
        {
            // Hangfire Configuration //
            var storageServer = new SqlServerStorage("jobcnn", new SqlServerStorageOptions() { 
                PrepareSchemaIfNecessary = true,
            });
            JobStorage.Current = storageServer;
            app.UseHangfireDashboard("/scheduler-jobs", new DashboardOptions() {
               Authorization = new []{ new HangfireAuthorizationFilter() }
            }, storageServer);

            var bgJobOptions = new BackgroundJobServerOptions{ };
            app.UseHangfireServer(bgJobOptions, storageServer);
            initializeAllJobs();
        }

        public static void initializeAllJobs()
        {
            
            Task.Run(async () => {
                await Task.FromResult(0);

                TenantManager tenantMgr = new TenantManager(ApplicationContext.Create());
                List<Tenant> tList = tenantMgr.GetTenantList().Where(x=>x.Deleted==false).ToList();
                foreach (Tenant tenant in tList)
                {
                    ApplicationContext context = ApplicationContext.CreateContext(tenant.TenantID.ToString());
                    TenantSchedulerManager tenantSchedulerManager = new TenantSchedulerManager(context);
                    List<TenantScheduler> tenantSchedulers = tenantSchedulerManager.Load();
                    foreach (var x in tenantSchedulers)
                    {
                        Type jobType = typeof(IJobScheduler).Assembly.GetTypes()
                        .FirstOrDefault(mytype => mytype.GetInterfaces().Contains(typeof(IJobScheduler)) && mytype.Name.Equals(x.JobType));
                        if (jobType == null)
                            continue;

                        IJobScheduler classObj = Activator.CreateInstance(jobType) as IJobScheduler;
                        if (!string.IsNullOrWhiteSpace(x.CronExpression))
                        {
                            try
                            {
                                RecurringJob.AddOrUpdate(tenant.AccountID + "." + x.JobType, () => classObj.Execute(x.TenantID), x.CronExpression);
                                ULog.WriteLog(tenant.AccountID + "." + x.JobType + " Added in HangFire for: " + "("+tenant.TenantID+")");
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                            }
                        }
                    }
                }
            });
        }
        public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                // In case you need an OWIN context, use the next line, `OwinContext` class
                // is the part of the `Microsoft.Owin` package.
                var owinContext = new OwinContext(context.GetOwinEnvironment());
                ApplicationContext uContext = owinContext.Get<ApplicationContext>();
               
                // Allow all authenticated users to see the Dashboard (potentially dangerous).
                return uContext.UserManager.IsAdmin(uContext.CurrentUser);
            }
        }

        
       
    }
}