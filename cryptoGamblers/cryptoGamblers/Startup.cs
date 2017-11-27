using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cryptoGamblers.Startup))]
namespace cryptoGamblers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
