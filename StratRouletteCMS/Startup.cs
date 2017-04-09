using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StratRouletteCMS.Startup))]
namespace StratRouletteCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
