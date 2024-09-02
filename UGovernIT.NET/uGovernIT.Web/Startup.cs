using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(uGovernIT.Web.Startup))]
namespace uGovernIT.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
