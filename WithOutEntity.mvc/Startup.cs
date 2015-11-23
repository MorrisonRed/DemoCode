using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(democode.mvc.Startup))]
namespace democode.mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
