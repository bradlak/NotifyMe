using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NotifyMe.Backend.Startup))]

namespace NotifyMe.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}