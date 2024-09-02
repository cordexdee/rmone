using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute("uGovernIT.Report.ReportStartup", typeof(uGovernIT.Report.Startup))]
namespace uGovernIT.Report
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
