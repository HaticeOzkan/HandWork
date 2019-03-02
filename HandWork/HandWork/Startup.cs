using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HandWork.Startup))]
namespace HandWork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
