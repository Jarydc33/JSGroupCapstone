using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SafestRouteApplication.Startup))]
namespace SafestRouteApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
