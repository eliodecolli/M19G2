using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(M19G2.Startup))]
namespace M19G2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
