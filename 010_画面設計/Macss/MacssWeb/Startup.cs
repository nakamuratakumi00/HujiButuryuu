using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MacssWeb.Startup))]
namespace MacssWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
