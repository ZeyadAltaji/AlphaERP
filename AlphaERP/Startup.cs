using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AlphaERP.Startup))]
namespace AlphaERP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
