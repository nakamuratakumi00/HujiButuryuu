using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Macss.Startup))]

namespace Macss
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigurationAuth(app);
        }
    }
}
