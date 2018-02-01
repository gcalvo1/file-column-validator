using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CCubed_2012.Startup))]
namespace CCubed_2012
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
