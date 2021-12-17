using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JetMedicalWebApp.Startup))]
namespace JetMedicalWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
