using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WorldIndicators.Presentation.WebClient.Startup))]
namespace WorldIndicators.Presentation.WebClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
