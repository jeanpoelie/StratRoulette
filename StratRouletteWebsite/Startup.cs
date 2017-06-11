using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StratRouletteWebsite.Startup))]
namespace StratRouletteWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
