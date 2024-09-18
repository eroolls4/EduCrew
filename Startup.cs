using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EduCrew.Startup))]
namespace EduCrew
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
