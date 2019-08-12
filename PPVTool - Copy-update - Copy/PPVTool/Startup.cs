using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PPVTool.Startup))]
namespace PPVTool
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
