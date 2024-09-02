using Hangfire;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Manager.JobSchedulers;

namespace uGovernIT.Report
{
    public class JobStart
    {
        public static void Start(IAppBuilder app)
        {
            // Hangfire Configuration //

            Hangfire.SqlServer.SqlServerStorageOptions hangfireOption = new Hangfire.SqlServer.SqlServerStorageOptions();
            hangfireOption.PrepareSchemaIfNecessary = false;
            GlobalConfiguration.Configuration.UseSqlServerStorage("jjobcnn", hangfireOption);
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            Type jobType = typeof(IReportJobScheduler);
            foreach (Type mytype in jobType.Assembly.GetTypes()
                 .Where(mytype => mytype.GetInterfaces().Contains(typeof(IReportJobScheduler))))
            {
                IReportJobScheduler classObj = Activator.CreateInstance(mytype) as IReportJobScheduler;
                RecurringJob.AddOrUpdate(() => classObj.Execute(), classObj.Duration);
            }
        }
    }
}