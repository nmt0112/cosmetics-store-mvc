using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyPhamStore.Startup))]
namespace MyPhamStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
